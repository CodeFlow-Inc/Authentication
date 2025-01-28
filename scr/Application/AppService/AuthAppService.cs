using Authentication.Aplication.DTOs;
using Authentication.Domain.Entities;
using Authentication.Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Aplication.AppService;

public class AuthAppService(
	IAuthRepository userRepository,
	ILogger<AuthAppService> logger,
	IConfiguration configuration,
	UserManager<ApplicationUser> userManager,
	IHttpContextAccessor httpContextAccessor) : IAuthAppService
{
	public async Task<List<ApplicationUser>> ListUsers()
	{
		List<ApplicationUser> listUsers = await userRepository.ListUsers();

		return listUsers;
	}

	public async Task<ApplicationUser> GetUserByIdAsync(int userId)
	{
		ApplicationUser user = await userRepository.GetUserByIdAsync(userId);

		if (user == null)
			throw new ArgumentException("Usuário não existe!");

		return user;
	}

	public async void UpdateUser(ApplicationUser user)
	{
		ApplicationUser findUser = await userRepository.GetUserByIdAsync(user.Id) ?? throw new ArgumentException("Usuário não encontrado");

		findUser.Email = user.Email;
		findUser.UserName = user.UserName;

		await userRepository.UpdateAsync(findUser);
	}

	public async Task<bool> DeleteUser(int userId)
	{
		ApplicationUser findUser = await userRepository.GetUserByIdAsync(userId);
		if (findUser == null)
			throw new ArgumentException("Usuário não encontrado");

		await userRepository.DeleteAsync(findUser);

		return true;
	}

	public async Task<bool> SignUp(SignUpDTO signUpDTO)
	{
		var userExists = await userManager.FindByNameAsync(signUpDTO.Username);
		if (userExists != null)
			throw new ArgumentException("Username already exists!");

		userExists = await userManager.FindByEmailAsync(signUpDTO.Email);
		if (userExists != null)
			throw new ArgumentException("Email already exists!");

		ApplicationUser user;

		user = new ApplicationUser()
		{
			Email = signUpDTO.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
			UserName = signUpDTO.Username
		};

		var result = await userManager.CreateAsync(user, signUpDTO.Password);

		if (!result.Succeeded)
			throw new ArgumentException("Cadastro do usuário falhou.");

		return true;
	}

	public async Task<SsoDTO> SignIn(SignInDTO signInDTO)
	{
		var user = await userManager.FindByNameAsync(signInDTO.Username);
		if (user == null)
			throw new ArgumentException("Usuário não encontrado.");

		if (!await userManager.CheckPasswordAsync(user, signInDTO.Password))
			throw new ArgumentException("Senha inválida.");

		var userRoles = await userManager.GetRolesAsync(user);

		var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};

		foreach (var userRole in userRoles)
		{
			authClaims.Add(new Claim(ClaimTypes.Role, userRole));
		}

		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]!));

		var token = new JwtSecurityToken(
			issuer: configuration["ValidIssuer"],
			audience: configuration["ValidAudience"],
			expires: DateTime.Now.AddHours(3),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

		return new SsoDTO(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
	}

	public async Task<ApplicationUser> GetCurrentUser()
	{
		ApplicationUser user = (await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User))!;
		return user;
	}
}
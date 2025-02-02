using Authentication.Aplication.DTOs;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;
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
	IUnitOfWork unitOfWork,
	ILogger<AuthAppService> logger,
	IConfiguration configuration,
	UserManager<ApplicationUser> userManager,
	IHttpContextAccessor httpContextAccessor) : IAuthAppService
{
	public async Task<ApplicationUser> GetUserByIdAsync(int userId)
	{
		var user = await unitOfWork.AuthRepository.GetByIdAsync(userId);

		return user ?? throw new ArgumentException("Usuário não existe!");
	}

	public async void UpdateUser(ApplicationUser user)
	{
		ApplicationUser findUser = await unitOfWork.AuthRepository.GetByIdAsync(user.Id) 
			?? throw new ArgumentException("Usuário não encontrado");

		findUser.Email = user.Email;
		findUser.UserName = user.UserName;

		await unitOfWork.BeginTransactionAsync();
		await unitOfWork.AuthRepository.UpdateAsync(findUser, findUser.Id);
		await unitOfWork.CommitAsync();
	}

	public async Task<bool> DeleteUser(int userId)
	{
		ApplicationUser findUser = await unitOfWork.AuthRepository.GetByIdAsync(userId) 
			?? throw new ArgumentException("Usuário não encontrado");

		await unitOfWork.BeginTransactionAsync();
		await unitOfWork.AuthRepository.DeleteAsync(findUser, findUser.Id);
		await unitOfWork.CommitAsync();

		return true;
	}

	public async Task<bool> SignUp(SignUpDTO signUpDTO)
	{
		var userExists = await userManager.FindByNameAsync(signUpDTO.Username);
		if (userExists != null)
		{
			logger.LogInformation($"Usuário {signUpDTO.Username} já existe em nosso banco de dados");
			throw new ArgumentException("Usuário já existe!");
		}

		userExists = await userManager.FindByEmailAsync(signUpDTO.Email);
		if (userExists != null)
		{
			logger.LogInformation($"Email {signUpDTO.Email} já existe em nosso banco de dados");
			throw new ArgumentException("Esse email já existe em nossa base!");
		}

		if (signUpDTO.Password != signUpDTO.PasswordConfirm)
		{
			logger.LogInformation($"As senhas não se coincidem!");
			throw new ArgumentException("As senhas estão incorretas!");
		}
			
		var user = new ApplicationUser()
		{
			Email = signUpDTO.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
			UserName = signUpDTO.Username
		};

		await unitOfWork.BeginTransactionAsync();
		var result = await userManager.CreateAsync(user, signUpDTO.Password);

		if (!result.Succeeded)
		{
			await unitOfWork.RollbackAsync();
			logger.LogInformation($"Cadastro do usuário {signUpDTO.Username} falhou!");
			throw new ArgumentException("Cadastro do usuário falhou.");
		}
		await unitOfWork.CommitAsync();

		return true;
	}

	public async Task<SsoDTO> SignIn(SignInDTO signInDTO)
	{
		var user = await userManager.FindByNameAsync(signInDTO.Username);
		if (user == null)
		{
			logger.LogInformation($"Usuário {signInDTO.Username} não existe em nosso banco!");
			throw new ArgumentException("Usuário não encontrado.");
		}
			

		if (!await userManager.CheckPasswordAsync(user, signInDTO.Password))
		{
			logger.LogInformation("Senha informada incorreta!");
			throw new ArgumentException("Senha inválida.");
		}
			
		var userRoles = await userManager.GetRolesAsync(user);

		var authClaims = new List<Claim>
		{
			new(ClaimTypes.Name, user.UserName),
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Email, user.Email),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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

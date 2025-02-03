using Authentication.Aplication.UseCase.Register;
using Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Authentication.Aplication.UseCase.Login.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Authentication.Aplication.UseCase.Login.Response;
using CodeFlow.Start.Package.WebTransfer.Base;

namespace Authentication.Aplication.UseCase.Login;
public class LoginUseCase(
	IConfiguration configuration,
	UserManager<ApplicationUser> userManager,
	ILogger<RegisterUseCase> logger) : IRequestHandler<LoginRequest, LoginResponse>
{
	public async Task<LoginResponse> Handle(
		LoginRequest request,
		CancellationToken cancellationToken)
	{
		var response = new LoginResponse();

		var user = await userManager.FindByNameAsync(request.Username);
		if (user == null)
		{
			logger.LogInformation("Usuário {Username} não existe em nosso banco!", request.Username);
			return response.AddErrorMessage<LoginResponse>("Usuário não encontrado.", ErrorType.InvalidCredentials);
		}


		if (!await userManager.CheckPasswordAsync(user, request.Password))
		{
			logger.LogInformation("Senha informada incorreta!");
			return response.AddErrorMessage<LoginResponse>("Senha inválida.", ErrorType.InvalidCredentials);
		}

		var userRoles = await userManager.GetRolesAsync(user);

		var authClaims = new List<Claim>
		{
			new(ClaimTypes.Name, user.UserName!),
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Email, user.Email!),
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

		response.SetResult(new(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo));
		return response;
	}
}
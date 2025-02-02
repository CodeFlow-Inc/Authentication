using MediatR;
using Authentication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Authentication.Infrastructure.Persistence;
using Authentication.Aplication.UseCase.Register.Dto.Request;
using Authentication.Aplication.UseCase.Register.Dto.Response;

namespace Authentication.Aplication.UseCase.Register;

public class RegisterUserUseCase(
	IUnitOfWork unitOfWork,
	UserManager<ApplicationUser> userManager,
	ILogger<RegisterUserUseCase> logger) : IRequestHandler<RegisterRequest, RegisterReponse>
{
	public async Task<RegisterReponse> Handle(
		RegisterRequest request,
		CancellationToken cancellationToken)
	{
		var response = new RegisterReponse();

		var existingUser = await userManager.FindByNameAsync(request.Username);
		if (existingUser != null)
		{
			logger.LogInformation("Username {Username} já existe.", request.Username);
			return response.AddErrorMessage<RegisterReponse>("Usuário já existe.");
		}

		existingUser = await userManager.FindByEmailAsync(request.Email);
		if (existingUser != null)
		{
			logger.LogInformation("Email {Email} já existe.", request.Email);
			response.AddErrorMessage<RegisterReponse>("Email já cadastrado.");
		}

		var user = ApplicationUser.Create(request.Username, request.Email);
		var result = await userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			await unitOfWork.RollbackAsync();
			logger.LogInformation("Cadastro do usuário {Username} falhou!", request.Username);
			return response.AddErrorMessage<RegisterReponse>("Cadastro do usuário falhou.");
		}

		response.SetResult(result.Succeeded);
		return response;
	}
}
using MediatR;
using Authentication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Authentication.Infrastructure.Persistence;
using Authentication.Aplication.UseCase.Register.Response;
using Authentication.Aplication.UseCase.Register.Request;

namespace Authentication.Aplication.UseCase.Register;

public class RegisterUseCase(
	IUnitOfWork unitOfWork,
	UserManager<ApplicationUser> userManager,
	ILogger<RegisterUseCase> logger) : IRequestHandler<RegisterRequest, RegisterResponse>
{
	public async Task<RegisterResponse> Handle(
		RegisterRequest request,
		CancellationToken cancellationToken)
	{
		var response = new RegisterResponse();

		var existingUser = await userManager.FindByNameAsync(request.Username);
		if (existingUser != null)
		{
			logger.LogInformation("Username {Username} já existe.", request.Username);
			return response.AddErrorMessage<RegisterResponse>("Usuário já existe.");
		}

		existingUser = await userManager.FindByEmailAsync(request.Email);
		if (existingUser != null)
		{
			logger.LogInformation("Email {Email} já existe.", request.Email);
			response.AddErrorMessage<RegisterResponse>("Email já cadastrado.");
		}

		var user = ApplicationUser.Create(request.Username, request.Email);
		var result = await userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			await unitOfWork.RollbackAsync();
			logger.LogInformation("Cadastro do usuário {Username} falhou!", request.Username);
			return response.AddErrorMessage<RegisterResponse>("Cadastro do usuário falhou.");
		}

		response.SetResult(result.Succeeded);
		return response;
	}
}
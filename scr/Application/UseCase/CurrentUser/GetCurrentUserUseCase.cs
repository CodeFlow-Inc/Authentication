using Authentication.Aplication.UseCase.CurrentUser.Request;
using Authentication.Aplication.UseCase.CurrentUser.Response;
using Authentication.Aplication.UseCase.Register;
using Authentication.Domain.Entities;
using CodeFlow.Start.Package.WebTransfer.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Authentication.Aplication.UseCase.CurrentUser;
public class GetCurrentUserUseCase(
	UserManager<ApplicationUser> userManager,
	IHttpContextAccessor httpContextAccessor,
	ILogger<RegisterUseCase> logger) : IRequestHandler<CurrentUserRequest, CurrentUserResponse>
{ 
	public async Task<CurrentUserResponse> Handle(
		CurrentUserRequest request,
		CancellationToken cancellationToken)
	{
		var response = new CurrentUserResponse();

		ApplicationUser? user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		if (user == null)
		{
			logger.LogInformation("Usuário não encontrado!");
			return response.AddErrorMessage<CurrentUserResponse>("Usuário não encontrado.", ErrorType.InvalidCredentials);
		}

		response.SetResult(new(user.Id, user.NormalizedUserName!, user.NormalizedEmail!));
		return response;
	}
}

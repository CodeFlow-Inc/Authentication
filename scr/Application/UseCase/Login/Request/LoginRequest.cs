using Authentication.Aplication.UseCase.Login.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Aplication.UseCase.Login.Request;

public record LoginRequest(
	[property: Required(ErrorMessage = "Campo usuário é obrigatório")] string Username,
	[property: Required(ErrorMessage = "Campo senha é obrigatório")] string Password) : IRequest<LoginResponse>;
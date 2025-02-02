using Authentication.Aplication.UseCase.Register.Dto.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Aplication.UseCase.Register.Dto.Request;

public record RegisterRequest(
	[property: Required(ErrorMessage = "Nome do usuário é obrigatório")] string Username,
	[property: EmailAddress][property: Required(ErrorMessage = "Email é obrigatório")] string Email,
	[property: Required(ErrorMessage = "Senha é obrigatório")] string Password,
	[property: Required(ErrorMessage = "Confirmação da senha é obrigatório")] string PasswordConfirm) : IRequest<RegisterReponse>;

using System.ComponentModel.DataAnnotations;

namespace Authentication.Aplication.DTOs;

public class SignUpDTO
{
	[Required(ErrorMessage = "Nome do usuário é obrigatório")]
	public required string Username { get; set; }

	[EmailAddress]
	[Required(ErrorMessage = "Email é obrigatório")]
	public required string Email { get; set; }

	[Required(ErrorMessage = "Senha é obrigatório")]
	public required string Password { get; set; }

	[Required(ErrorMessage = "Confirmação da senha é obrigatório")]
	public required string PasswordConfirm { get; set; }
}

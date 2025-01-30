using System.ComponentModel.DataAnnotations;

namespace Authentication.Aplication.DTOs;

public class SignInDTO
{
	[Required(ErrorMessage = "Campo usuário é obrigatório")]
	public required string Username { get; set; }

	[Required(ErrorMessage = "Campo senha é obrigatório")]
	public required string Password { get; set; }
}

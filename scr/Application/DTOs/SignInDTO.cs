using System.ComponentModel.DataAnnotations;

namespace Authentication.Aplication.DTOs;

public class SignInDTO
{
	[Required(ErrorMessage = "User Name is required")]
	public required string Username { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public required string Password { get; set; }
}

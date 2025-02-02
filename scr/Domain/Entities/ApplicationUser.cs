using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
	private ApplicationUser(string username, string email)
	{
		UserName = username;
		Email = email;
	}

	internal ApplicationUser(int id, string username, string email)
	{
		Id = id;
		UserName = username;
		Email = email;
	}

	public ApplicationUser()
	{
	}

	public static ApplicationUser Create(string username, string email)
	{
		if (string.IsNullOrWhiteSpace(username))
			throw new ArgumentException("Username não pode ser vazio.");

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentException("Email não pode ser vazio.");

		if (!IsValidEmail(email))
			throw new ArgumentException("Email inválido.");

		return new ApplicationUser(username, email);
	}

	private static bool IsValidEmail(string email)
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}

	internal void SetId(int id)
	{
		if (Id != 0)
			throw new InvalidOperationException("Id já foi definido.");
		Id = id;
	}
}

using System.IdentityModel.Tokens.Jwt;

namespace Authentication.Domain.Identity;

public class TokenJWT
{
	private readonly JwtSecurityToken Token;

	internal TokenJWT(JwtSecurityToken token)
	{
		this.Token = token;
	}

	public DateTime ValidTo => Token.ValidTo;

	public string Value => new JwtSecurityTokenHandler().WriteToken(this.Token);
}

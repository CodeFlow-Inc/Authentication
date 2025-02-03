using CodeFlow.Start.Package.WebTransfer.Base.Response;

namespace Authentication.Aplication.UseCase.Login.Response;
public class LoginResponse : BaseResponse, IResultResponse<SsoResponse>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public SsoResponse? Result { get; set; } = default;

	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(SsoResponse result)
	{
		Result = result;
	}
}

public class SsoResponse
{
	public string AccessToken { get; private set; }
	public DateTime Expiration { get; private set; }

	public SsoResponse(string accessToken, DateTime expiration)
	{
		this.AccessToken = accessToken;
		this.Expiration = expiration;
	}
}

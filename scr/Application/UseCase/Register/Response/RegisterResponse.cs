using CodeFlow.Start.Package.WebTransfer.Base.Response;

namespace Authentication.Aplication.UseCase.Register.Response;
public class RegisterResponse : BaseResponse, IResultResponse<bool>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public bool Result { get; set; } = default;

	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(bool result)
	{
		Result = result;
	}
}

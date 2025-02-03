using CodeFlow.Start.Package.WebTransfer.Base.Response;

namespace Authentication.Aplication.UseCase.CurrentUser.Response;
public class CurrentUserResponse : BaseResponse, IResultResponse<CurrentUserResult>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public CurrentUserResult? Result { get; set; } = default;
	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(CurrentUserResult result)
	{
		Result = result;
	}
}

public class CurrentUserResult
{
	/// <summary>
	/// The result value of the use case.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userName"></param>
	/// <param name="email"></param>
	public CurrentUserResult(int id, string userName, string email)
	{
		this.Id = id;
		this.UserName = userName;
		this.Email = email;
	}

	public int Id { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
}
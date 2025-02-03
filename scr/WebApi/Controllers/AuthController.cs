using CodeFlow.Start.Package.WebTransfer.Base.Response;
using CodeFlow.Start.Package.WebTransfer.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Authentication.Aplication.UseCase.Register.Request;
using Authentication.Aplication.UseCase.Login.Request;
using Authentication.Aplication.UseCase.Login.Response;
using Authentication.Aplication.UseCase.CurrentUser.Request;
using Authentication.Aplication.UseCase.CurrentUser.Response;

namespace Authentication.WebApi.Controllers;

[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
	/// <summary>
	/// Registers a new user in the system.
	/// </summary>
	/// <param name="request">The registration request containing user details.</param>
	/// <returns>Returns a success response if the registration is successful or an error response if it fails.</returns>
	[AllowAnonymous]
	[HttpPost("register")]
	[SwaggerOperation(
		Summary = "Register a new user",
		Description = "Creates a new user account with the provided information. The request must contain valid user details."
	)]
	[SwaggerResponse(StatusCodes.Status200OK, "User successfully registered.", typeof(FileResult))]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or business rule violation.", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error.", typeof(BaseResponse))]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		var response = await mediator.Send(request);

		if (!response.IsSuccess)
		{
			return response.ErrorType switch
			{
				ErrorType.BusinessRuleError => BadRequest(response),
				_ => StatusCode(StatusCodes.Status500InternalServerError, response)
			};
		}
		return Ok(response);
	}

	/// <summary>
	/// Authenticates a user and generates an access token.
	/// </summary>
	/// <param name="request">The login request containing user credentials.</param>
	/// <returns>Returns an access token if authentication is successful, or an error response if authentication fails.</returns>
	[AllowAnonymous]
	[HttpPost("login")]
	[SwaggerOperation(
		Summary = "Authenticate user",
		Description = "Validates user credentials and generates a JWT access token upon successful authentication."
	)]
	[SwaggerResponse(StatusCodes.Status200OK, "User successfully authenticated.", typeof(LoginResponse))]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid credentials or request format.", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication failed due to invalid credentials.", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error.", typeof(BaseResponse))]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		var response = await mediator.Send(request);

		if (!response.IsSuccess)
		{
			return response.ErrorType switch
			{
				ErrorType.InvalidCredentials => Unauthorized(response),
				ErrorType.BusinessRuleError => BadRequest(response),
				_ => StatusCode(StatusCodes.Status500InternalServerError, response)
			};
		}
		return Ok(response);
	}

	/// <summary>
	/// Retrieves the current authenticated user's details.
	/// </summary>
	/// <returns>Returns the current user's details or an error response if the user is not found.</returns>
	[HttpGet("current-user")]
	[SwaggerOperation(
		Summary = "Get current authenticated user",
		Description = "Retrieves details of the currently authenticated user."
	)]
	[SwaggerResponse(StatusCodes.Status200OK, "User details retrieved successfully.", typeof(CurrentUserResponse))]
	[SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error.", typeof(BaseResponse))]
	public async Task<IActionResult> GetCurrentUser()
	{
		var response = await mediator.Send(new CurrentUserRequest());

		if (!response.IsSuccess)
		{
			return response.ErrorType switch
			{
				ErrorType.InvalidCredentials => Unauthorized(response),
				_ => StatusCode(StatusCodes.Status500InternalServerError, response)
			};
		}
		return Ok(response);
	}
}

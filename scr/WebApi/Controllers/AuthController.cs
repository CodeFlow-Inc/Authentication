using Authentication.Aplication.AppService;
using Authentication.Aplication.DTOs;
using Authentication.Domain.Entities;
using CodeFlow.Start.Package.WebTransfer.Base.Response;
using CodeFlow.Start.Package.WebTransfer.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Authentication.Aplication.UseCase.Register.Dto.Request;

namespace Authentication.WebApi.Controllers;

[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController(IAuthAppService authService, IMediator mediator) : ControllerBase
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

	[AllowAnonymous]
	[HttpPost("sign-up")]
	public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
	{
		try
		{
			bool ret = await authService.SignUp(signUpDTO);

			return Ok(ret);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[AllowAnonymous]
	[HttpPost("sign-in")]
	public async Task<ActionResult> SignIn([FromBody] SignInDTO signInDTO)
	{
		try
		{
			SsoDTO ssoDTO = await authService.SignIn(signInDTO);

			return Ok(ssoDTO);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("get-current-user")]
	public async Task<ActionResult> GetCurrentUser()
	{
		try
		{
			ApplicationUser currentUser = await authService.GetCurrentUser();

			return Ok(currentUser);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}

using Authentication.Aplication.AppService;
using Authentication.Aplication.DTOs;
using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers;

[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController(IAuthAppService authService) : ControllerBase
{
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

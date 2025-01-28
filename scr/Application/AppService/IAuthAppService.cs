using Authentication.Aplication.DTOs;
using Authentication.Domain.Entities;

namespace Authentication.Aplication.AppService;

public interface IAuthAppService
{
	Task<bool> SignUp(SignUpDTO signUpDTO);
	Task<SsoDTO> SignIn(SignInDTO signInDTO);
	Task<ApplicationUser> GetCurrentUser();
	Task<bool> DeleteUser(int userId);
	void UpdateUser(ApplicationUser user);
	Task<ApplicationUser> GetUserByIdAsync(int userId);
	Task<List<ApplicationUser>> ListUsers();
}

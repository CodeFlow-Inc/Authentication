using Authentication.Application.DTOs;
using Authentication.Domain.Entities;

namespace Authentication.Application.Interface
{
    public interface IAuthApplication
    {
        Task<bool> SignUp(SignUpDTO signUpDTO);
        Task<SsoDTO> SignIn(SignInDTO signInDTO);
        Task<ApplicationUser> GetCurrentUser();
        Task<bool> DeleteUser(string userId);
        void UpdateUser(ApplicationUser user);
        Task<ApplicationUser> GetUserById(string userId);
        Task<List<ApplicationUser>> ListUsers();
    }
}

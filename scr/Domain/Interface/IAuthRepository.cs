using Authentication.Domain.Entities;

namespace Authentication.Domain.Interface;

public interface IAuthRepository : IBaseRepository<ApplicationUser>
{
	Task<ApplicationUser> GetUserByIdAsync(int userId);
	Task<List<ApplicationUser>> ListUsers();
}
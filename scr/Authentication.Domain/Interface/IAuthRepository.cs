using Authentication.Domain.Entities;

namespace Authentication.Infrasctuture.Interface
{
    public interface IAuthRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUser(string userId);
        Task<List<ApplicationUser>> ListUsers();
    }
}
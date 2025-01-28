using Authentication.Domain.Entities;
using Authentication.Infrasctuture.Interface;
using Authentication.Infrasctuture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrasctuture.Repositories
{
    public class AuthRepository : BaseRepository<ApplicationUser>, IAuthRepository
    {
        public AuthRepository(SqlContext context) : base(context) { }
        public async Task<List<ApplicationUser>> ListUsers()
        {
            List<ApplicationUser> list = await _context.Users.ToListAsync();

            return list;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            ApplicationUser user = await _context.Users.FindAsync(userId);

            return user;
        }
    }
}
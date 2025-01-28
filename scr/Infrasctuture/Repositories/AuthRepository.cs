using Authentication.Domain.Entities;
using Authentication.Domain.Interface;
using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Repositories;

public class AuthRepository : BaseRepository<ApplicationUser>, IAuthRepository
{
	public AuthRepository(SqlContext context) : base(context) { }
	public async Task<List<ApplicationUser>> ListUsers()
	{
		List<ApplicationUser> list = await _context.Users.ToListAsync();

		return list;
	}

	public async Task<ApplicationUser> GetUserByIdAsync(int userId)
	{
		ApplicationUser user = await _context.Users.FindAsync(userId);

		return user;
	}
}
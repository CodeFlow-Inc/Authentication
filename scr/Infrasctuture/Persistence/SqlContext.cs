using Authentication.Domain.Entities;
using Authentication.Infrastructure.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence;

public class SqlContext(DbContextOptions<SqlContext> opts)
	: IdentityDbContext<ApplicationUser, ApplicationRole, int>(opts)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new ApplicationUserMap());
	}
}

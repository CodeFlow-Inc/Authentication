using Authentication.Domain.Entities;
using Authentication.Infrastructure.Mappings;
using CodeFlow.Data.Context.Package.Base.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence;

public class SqlContext(DbContextOptions<SqlContext> opts)
	: BaseIdentityDbContex<ApplicationUser, ApplicationRole, int>(opts)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new ApplicationUserMap());
	}
}

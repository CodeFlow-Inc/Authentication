using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Authentication.Infrasctuture.Persistence;

public class SqlContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public SqlContext(DbContextOptions<SqlContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new Mappings.ApplicationUserMap());
    }
}

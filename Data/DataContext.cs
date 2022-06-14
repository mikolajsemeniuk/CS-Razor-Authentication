using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : IdentityDbContext<Account, Role, Guid,
                               IdentityUserClaim<Guid>, AccountRole, IdentityUserLogin<Guid>,
                               IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


  


        builder.Entity<Account>()
            .HasMany(account => account.AccountRoles)
            .WithOne(accountRole => accountRole.Account)
            .HasForeignKey(account => account.UserId)
            .IsRequired();

        builder.Entity<Role>()
            .HasMany(role => role.AccountRoles)
            .WithOne(accountRole => accountRole.Role)
            .HasForeignKey(account => account.RoleId)
            .IsRequired();
    }
}
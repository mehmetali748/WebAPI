using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1) AspNetUserRoles → AspNetUsers ilişkisi için silme davranışını "Restrict" veya "NoAction" yap
            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            // 2) AspNetUserRoles → AspNetRoles ilişkisi için silme davranışını "Restrict" veya "NoAction" yap
            builder.Entity<ApplicationUserRole>(roleUser =>
            {
                roleUser.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            // (İsteğe bağlı) Diğer Identity ilişki tabloları için de aşağıdaki gibi Cascade’i kapatabilirsiniz:
            // builder.Entity<ApplicationRoleClaim>()
            //        .HasOne(rc => rc.Role)
            //        .WithMany()
            //        .HasForeignKey(rc => rc.RoleId)
            //        .OnDelete(DeleteBehavior.Restrict);
            //
            // builder.Entity<ApplicationUserClaim>()
            //        .HasOne(uc => uc.User)
            //        .WithMany()
            //        .HasForeignKey(uc => uc.UserId)
            //        .OnDelete(DeleteBehavior.Restrict);
            //
            // vb...
        }
    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid> { }
    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }
    public class ApplicationUserToken : IdentityUserToken<Guid> { }
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid> { }
}

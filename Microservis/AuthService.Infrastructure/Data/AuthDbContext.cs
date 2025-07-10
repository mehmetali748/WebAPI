using AuthService.Domain.Enties;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<
      ApplicationUser,       // TUser
      ApplicationRole,       // TRole
      Guid,                  // TKey tipi
      ApplicationUserClaim,  // TUserClaim
      ApplicationUserRole,   // TUserRole
      ApplicationUserLogin,  // TUserLogin
      ApplicationRoleClaim,  // TRoleClaim
      ApplicationUserToken   // TUserToken
  >
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Örneğin, tabloların isimlerini özelleştirmek isterseniz:
            // builder.Entity<ApplicationUser>().ToTable("Users");
            // builder.Entity<ApplicationRole>().ToTable("Roles");
            // builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
        }
    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid> { }
    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }
    public class ApplicationUserToken : IdentityUserToken<Guid> { }
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid> { }
}

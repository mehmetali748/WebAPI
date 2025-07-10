using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthService.Domain.Entities
{
    public class ApplicationUser :IdentityUser<Guid>

    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;

    }
}

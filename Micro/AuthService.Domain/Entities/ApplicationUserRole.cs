using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class ApplicationUserRole:IdentityUserRole<Guid>

    {
        public virtual ApplicationRole Role { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}

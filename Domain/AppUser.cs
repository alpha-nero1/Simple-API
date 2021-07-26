using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    // Dervied app user class gives us full functionality from Microsoft.AspNetCore.Identity.EntityFrameworkCore.
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<PostUser> PostUsers { get; set; }
    }
}

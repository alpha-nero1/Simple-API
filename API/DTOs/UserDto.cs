using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    // What we return to client after login/register.
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}

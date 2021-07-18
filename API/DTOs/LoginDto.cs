using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    // What we requre from user to log in.
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

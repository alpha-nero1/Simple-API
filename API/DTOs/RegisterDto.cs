using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    // What we require from user to register.
    // Okay to validate with data annotations here because it is a quick and easy way to
    // validate at the API level (this is a DTO after all and not a DB object).
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4, 8}$", ErrorMessage = "Password not complex enough")]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        public string Username { get; set; }
    }
}

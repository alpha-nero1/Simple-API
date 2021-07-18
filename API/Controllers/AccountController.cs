using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    // User does not need to auth on this controller!
    [AllowAnonymous]
    // Here we are using a controller for auth not using the CQRS pattern.
    // We are decoupling auth logic from the rest of our application.
    // This would allow us to change the auth logic without effecting the rest of the app.
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _uManager;
        private readonly SignInManager<AppUser> _sManager;
        private readonly ITokenService _tokenService;

        public AccountController(
            UserManager<AppUser> uManager, 
            SignInManager<AppUser> sManager,
            ITokenService tokenService
        )
        {
            _uManager = uManager;
            _sManager = sManager;
            _tokenService = tokenService;
        }

        // See what user is logged in from a token.
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            var emailJwtClaim = User.FindFirstValue(ClaimTypes.Email);
            if (emailJwtClaim == null) return NotFound();
            var user = await _uManager.FindByEmailAsync(emailJwtClaim);
            if (user == null) return NotFound();
            return CreateUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto req)
        {
            var user = await _uManager.FindByEmailAsync(req.Email);
            if (user == null) return Unauthorized();
            var res = await _sManager.CheckPasswordSignInAsync(user, req.Password, false);
            if (res.Succeeded)
            {
                return CreateUserDto(user);
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto req)
        {
            var emailExists = await _uManager.Users.AnyAsync(x => x.Email == req.Email);
            if (emailExists) return BadRequest("Email already in use by another user.");
            var usernameExists = await _uManager.Users.AnyAsync(x => x.UserName == req.Username);
            if (usernameExists) return BadRequest("Username already in use by another user.");
            // Create user.
            var user = new AppUser
            {
                DisplayName = req.DisplayName,
                Email = req.Email,
                UserName = req.Username
            };
            var res = await _uManager.CreateAsync(user, req.Password);
            if (res.Succeeded) return CreateUserDto(user);
            return BadRequest("Issue registering new user.");
        }

        private UserDto CreateUserDto(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                AccessToken = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}

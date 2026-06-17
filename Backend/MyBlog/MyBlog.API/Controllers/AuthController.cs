using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyBlog.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using MyBlog.Application.Dtos;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            // Ensure username and email are unique
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
            {
                return Conflict(new { message = "Username already exists." });
            }

            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return Conflict(new { message = "Email already exists." });
            }

            var user = new ApplicationUser 
            {
                FullName = dto.FullName, 
                UserName = dto.UserName, 
                Email = dto.Email 
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role;
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Allow login by either username or email
            var identifier = dto.UserName?.Trim();
            if (string.IsNullOrEmpty(identifier)) return Unauthorized();

            var user = await _userManager.FindByNameAsync(identifier);
            if (user == null && identifier.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }

            if (user == null) return Unauthorized();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new 
            { 
                token,
                user = new
                {
                    //id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    fullName = user.FullName,
                    roles
                }
            });
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                fullName = user.FullName,
                roles
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    fullName = user.FullName,
                    roles
                });
            }

            return Ok(userList);
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var key = _config["Jwt:Key"] ?? "ThisIsASecretKeyForDevOnlyReplaceInProduction";
            var issuer = _config["Jwt:Issuer"] ?? "MyBlogApi";
            var audience = _config["Jwt:Audience"] ?? "MyBlogClient";

            var keyBytes = Encoding.UTF8.GetBytes(key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName ?? user.UserName ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

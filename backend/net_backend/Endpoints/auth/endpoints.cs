using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomAttributes;
using CustomExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using RequestObjects;

namespace EndpointAuth
{

    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _auth;
        private readonly IConfiguration _config;
        public Endpoint(AppDbContext context, AuthService auth, IConfiguration config)
        {
            _context = context;
            _auth = auth;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> PostLogin(Login userlogin)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? "dummy"));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userlogin.Username);
            if (user == null || !_auth.VerifyPassword(user.PwHash, userlogin.Password))
            {
                return Unauthorized("Invalid Credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            return user;
        }

        [HttpGet("devlogin")]
        public async Task<ActionResult<User>> GetLogin([FromQuery] Login userlogin)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? "dummy"));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userlogin.Username);
            if (user == null || !_auth.VerifyPassword(user.PwHash, userlogin.Password))
            {
                return Unauthorized("Invalid Credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            return user;
        }

        [HttpGet("devregister")]
        public async Task<ActionResult<User>> GetRegister([FromQuery] Login userlogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userlogin.Username);
            if (user != null)
            {
                return Conflict("Username already taken");
            }

            var newuser = await _context.Users.AddAsync(new User
            {
                Username = userlogin.Username,
                PwHash = _auth.HashPassword(userlogin.Password),
                Email = "mail@example.com"
            });

            await _context.SaveChangesAsync();

            
            return newuser.Entity;
        }

        [HttpGet("logout")]
        public  ActionResult<string> GetLogout()
        {
            Response.Cookies.Delete("jwt");
            return "you are logged out";
        }

        [HttpGet("asroot")]
        public  ActionResult<string> GetAsRoot()
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? "dummy"));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "root"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            return "you are root now";
        }

        

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<User>>> GetUsers([FromQuery] PaginationParams paging)
        {
            var users = await _context.Users.OrderBy(x => x.Id).PaginateAsync(paging);
            return users;
        }
    }

}
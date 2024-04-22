using Microsoft.AspNetCore.Mvc;
using Flightmanager.Login.Data;
using Flightmanager.Login.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Flightmanager.Login.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class Usercontroler : ControllerBase
    {
        private readonly UserApiContext _configuration;

        public Usercontroler(UserApiContext configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            request.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.PasswordHash, "$2a$10$vwjDYzovdAjC1NOElQmSHO");
            User req = new User();
            req.PasswordHash = request.PasswordHash;
            req.Username = request.Username;
            var userInDb = _configuration.Users.Find(req.Username);
            if (userInDb != null)
            {
                return Conflict();
            }

            _configuration.Users.Add(req);
            _configuration.SaveChanges();

            return Ok(req);
        }

        [HttpPost("login")]
        public ActionResult Login(UserDto request)
        {
            request.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.PasswordHash, "$2a$10$vwjDYzovdAjC1NOElQmSHO");
            User req = new User();
            req.PasswordHash = request.PasswordHash;
            req.Username = request.Username;
            var userInDb = _configuration.Users.Find(req.Username);
            if (userInDb == null)
            {
                return NotFound("User not found"); 
            }

            if (request.PasswordHash != userInDb.PasswordHash)
            {
                return BadRequest("Incorrect password");
            }

            string token = CreateToken(req);
            
            var refreshToken = GenerateRefreshToken();
            _configuration.SaveChanges();
            SetRefreshToken(refreshToken, userInDb);
            
            
            

            return Ok(new { token = token });
        }


        [HttpGet("ref_refresh_token")]
        public async  Task<ActionResult> RefreshToken()
        {
            var refreshTokenValue = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshTokenValue))
            {
                return BadRequest("Refresh token is missing.");
            }

            var user = _configuration.Users.FirstOrDefault(u => u.RefreshToken == refreshTokenValue);

            if (user == null)
            {
                return BadRequest("there is no that user");
            }
            else if (user.TokenExpired < DateTime.Now)
            {
                return Unauthorized("Expiered Token");
            }

            string token = CreateToken(user);
            RefreshTocken newRefreshToken = GenerateRefreshToken();

            SetRefreshToken(newRefreshToken, user);
            return Ok(new { token = token });
        }

        private RefreshTocken GenerateRefreshToken()
        {
            var refreshToken = new RefreshTocken
            {
                Token = GenerateRandomString(64),
                Expires = DateTime.Now.AddDays(5)
            };

            return refreshToken;
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void SetRefreshToken(RefreshTocken newRefreshTocken, User userInDb)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshTocken.Expires
            };

            userInDb.RefreshToken = newRefreshTocken.Token;
            userInDb.TokenCreated = newRefreshTocken.Created;
            userInDb.TokenExpired = newRefreshTocken.Expires;
            _configuration.SaveChanges();
            Response.Cookies.Append("refreshToken", newRefreshTocken.Token, cookieOptions);

        }

        private string CreateToken(User user)
        {
            bool isAdmin = user.Username.ToLower() == "admin";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user")
            };
            string password = "asdsen123tokenaaokasdsdadadasdasdasdasdsen123token";
            string issuer = "http://localhost:5000";
            string audience = "http://localhost:5000";

            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var key = new SymmetricSecurityKey(passwordBytes);
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: cred
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

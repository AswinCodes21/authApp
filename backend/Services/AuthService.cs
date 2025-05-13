using Microsoft.EntityFrameworkCore;
using backend.DTOs;
using backend.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using backend.Models;

namespace backend.Services
{
    public class AuthService
    {
        // Add methods for user management and authentication
        private readonly IConfiguration _configuration;
        private readonly UserDbContext _context;

        public AuthService(IConfiguration configuration, UserDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public AuthResult RegisterUser(UserDTO userDto)
        {
            // Hash password and save user
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new AuthResult { Success = true, Message = "User registered successfully." };
        }

        public AuthResult AuthenticateUser(UserDTO userDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return new AuthResult { Success = false, Message = "Invalid credentials." };
            }

            var token = GenerateJwtToken(user);
            return new AuthResult { Success = true, Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AgriBoostAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        
        private string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("AgriBoostSecretKey");
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        
        public async Task<User?> RegisterUser(string name, string email, string password, string userType)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return null; // Email already registered

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password),
                UserType = userType
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
        public async Task<string?> AuthenticateUser(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordHash != HashPassword(password))
                return null; 

            return GenerateJwtToken(user);
        }

        
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType) 
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

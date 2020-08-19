using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Acreator.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthRepo(AppDbContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }

        public async Task<RepoResponse<AdminReturnDto>> Login(Admin credentials)
        {
            var admin = await AuthenticateAdmin(credentials.Email, credentials.Password);

            if (admin == null)
            {
                return new RepoResponse<AdminReturnDto>()
                {
                    Content = null,
                    IsSuccess = false,
                    StatusCode = 401
                };
            }

            return new RepoResponse<AdminReturnDto>()
            {
                Content = new AdminReturnDto()
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    token = GenerateToken(admin.Id, admin.Email)
                },
                IsSuccess = true,
                StatusCode = 200
            };
        }
        
        private string GenerateToken(int id, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim("role", "admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        private async Task<Admin> AuthenticateAdmin(string email, string password)
        {
            var admin = await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.Password == password);
            return admin;
        }
    }
}
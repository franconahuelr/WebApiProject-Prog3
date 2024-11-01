using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

namespace WebApiProject.Custom
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;
        private readonly DbApiProjectContext _dbApiProjectContext;

        public Utilities(IConfiguration configuration, DbApiProjectContext dbApiProjectContext)
        {
            _configuration = configuration;
            _dbApiProjectContext = dbApiProjectContext;
        }
        public string EncryptSHA256(string text)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convertir el array de bytes a string
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public string GenerateJWT(User user)
        {
            var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUser),
            new Claim(ClaimTypes.Email, user.Email)
        };

            // Obtener el rol del usuario
            var role = _dbApiProjectContext.Roles.Find(user.RoleId); // Usa FindAsync si es necesario
            if (role != null)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
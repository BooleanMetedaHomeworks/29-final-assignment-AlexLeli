using Microsoft.IdentityModel.Tokens;
using ristorante_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ristorante_backend.Services
{

    public class JwtAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public readonly JwtSettings _jwtSettings;
        private readonly UserService _userService;

        public JwtAuthenticationService(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            _userService = userService;
        }

        public async Task<string> Authenticate(string email, string password)
        {
            User user = await _userService.AuthenticateAsync(email, password);
            if (user == null) { return null; }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            List<string> roles = await _userService.GetUserRolesAsync(user.ID_User);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}

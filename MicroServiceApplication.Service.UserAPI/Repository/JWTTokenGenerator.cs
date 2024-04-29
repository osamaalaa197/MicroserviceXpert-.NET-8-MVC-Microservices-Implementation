using MicroServiceApplication.Service.UserAPI.IRepository;
using MicroServiceApplication.Service.UserAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroServiceApplication.Service.UserAPI.Repository
{
    public class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JWTTokenGenerator(IOptions<JwtOptions> jwtOptions) 
        {
            _jwtOptions=jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandeler = new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
            };
            claimList.AddRange(roles.Select(e => new Claim(ClaimTypes.Role, e)));
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience=_jwtOptions.Audience,
                Issuer=_jwtOptions.Issuer,
                Subject=new ClaimsIdentity(claimList),
                Expires=DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token=tokenHandeler.CreateToken(tokenDescriptor);
            return tokenHandeler.WriteToken(token);
        }
    }
}

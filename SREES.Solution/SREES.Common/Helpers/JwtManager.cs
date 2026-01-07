using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using SREES.Common.Models.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;

namespace SREES.Common.Helpers
{
    public class JwtManager
    {
        private static readonly string _secret = "5nh^r1uyXAs%Js5-(rOPZcj3zh{M40so=lDd)A+4h)-esH=st].e0:C;a)K|R?7n%ujeW.=._n?GXl5naj.ZBXEF|C%zqbXuWf3)7GpE)YR2hnud*b;[<iC{V*kSfm=vgJ;YH@JMeWSZ->}42lHBcZz:Kf{^2##!X_iZQ>6GH(>&riT&o1JMxR*q%+?y:x}V*Wp4$y=l!8:@}QvE#hQ)I(+aS<=dh.4I3-y.6|m^F)=*=@x{)XR(+uJE[!p*+A>>Y;";

        public static string GetToken(UserTokenDto user, int expirationTime = 120)
        {
            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "SREESUser"),
                    new Claim("id", user.Id.ToString()),
                    new Claim("userName", user.UserName),
                    new Claim("email", user.Email),
                    new Claim("role", user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationTime),
                SigningCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateToken(tokenDescriptor);

            return jwtHandler.WriteToken(token);
        }

        public static TokenValidationParameters GetTokenValidationParameters() 
        {
            return new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
            };
        }
    }
}

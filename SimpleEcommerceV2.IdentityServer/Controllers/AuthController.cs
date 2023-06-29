using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleEcommerceV2.IdentityServer.Domain.InOut;
using SimpleEcommerceV2.IdentityServer.Domain.Services.Contracts;

namespace SimpleEcommerceV2.IdentityServer.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController: ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync
        (
            [FromServices] IConfiguration configuration,
            [FromServices] IUserService userService,
            [FromBody] UserRequest request
        )
        {
            var isAuthenticated = await userService.CheckPasswordAsync(request);
            if (!isAuthenticated)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Identity:Key")));
            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.Email, request.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Identity:TokenLifetime")),
                Issuer = configuration.GetValue<string>("Identity:Issuer"),
                Audience = configuration.GetValue<string>("Identity:Audience"),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return Ok(new
            {
                token = jwt,
                expiration = token.ValidTo
            });
        }
    }
}

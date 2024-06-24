using CRUD.Data;
using CRUD.DDBBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace CRUD.API
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _authenticationToken;

        public AuthenticationController(IConfiguration config)
        {
            var apiTokenSection = config.GetSection("Setting").GetSection("Apitoken").ToString();
            _authenticationToken = apiTokenSection;
        }

        [HttpPost("Validar")]
        public IActionResult Validar([FromBody] Usuario user)
        {
            using(CRUDbContext context = new())
            {
                var usuario = context.Usuarios.Where(User => User.Correo == user.Correo).FirstOrDefault();
                if (usuario != null && usuario.Password == user.Password && usuario.IsActive == true) 
                {
                    var keyBytes = Encoding.ASCII.GetBytes(_authenticationToken);
                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Correo ?? string.Empty));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(5),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                    string token = tokenHandler.WriteToken(tokenConfig);

                    return StatusCode(StatusCodes.Status200OK, new { token = token });
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
        }
    }
}

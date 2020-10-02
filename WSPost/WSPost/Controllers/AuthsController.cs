using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WSPost.Models;

namespace WSPost.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        //Variable que guarda en confiración ambiente/Proteger la clave secreta
        //private readonly IConfiguration _configuration;

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Login user)
        {
            if (user == null)
                return BadRequest("Invalid client request!");

            if (user.UserName == "julius2Grow@gmail.com" && user.Password == "P@$$w0rd")
            {
                //Buena práctica es Guardar en variable ambiente, la clave secreta: _configuration 
                //evitar escribir en el appsettings por control de versiones, por error o accidente distribuya al repositorio, no es debido.
                //Si la publico en un servidor(Azure) el mismo nombre, pero diferente valor. 
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretTopKeyP@$$0369"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddHours(1);


                #region [ Roles ]
                //Rol por administrador o usuario
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(ClaimTypes.Role, "Manager")
                //}; 
                #endregion

                var tokenOptions = new JwtSecurityToken
                    (
                        issuer: "https://localhost:5001",
                        audience: "https://localhost:5001",
                        claims: new List<Claim>(),
                        expires: expiration,
                        signingCredentials: signingCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}

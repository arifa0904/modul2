using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using modul2.Models;
using modul2.RepositoriData;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace modul2.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly string __constr;
        private readonly IConfiguration _config;
/*      private readonly MuridContext _context;*/

        public AuthController(IConfiguration config)
        {
            _config = config;
            __constr = _config.GetConnectionString("koneksi");
           
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public IActionResult login([FromBody] login loginData)
        {
            MuridContext context = new MuridContext(__constr);
            Murid murid = context.GetMuridByEmail(loginData.Email);

            if (murid == null || murid.password != loginData.Password)
            {
                return Unauthorized(new { message = "Email atau password salah" });
            }

            JwtHelper jwtHelper = new JwtHelper(_config);
            var token = jwtHelper.GenerateToken(murid);

            return Ok(new
            {
                token = token,
                user = new
                {
                    id = murid.id_murid,
                    nama = murid.nama,
                    email = murid.email,
                   
                }
            });
        }



    }
}

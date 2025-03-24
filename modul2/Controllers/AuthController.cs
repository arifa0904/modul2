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
        private readonly IConfiguration _config;
        private readonly MuridContext _context;

        public AuthController(IConfiguration config)
        {
            _config = config;
            string connectionString = _config.GetConnectionString("koneksi");
            _context = new MuridContext(connectionString);
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public IActionResult Register([FromBody] Murid registerData)
        {
            if (string.IsNullOrEmpty(registerData.email) ||
                string.IsNullOrEmpty(registerData.password) ||
                string.IsNullOrEmpty(registerData.role))
            {
                return BadRequest(new { message = "Email, password, dan role harus diisi" });
            }

            Murid existingPerson = _context.GetMuridByEmail(registerData.email);
            if (existingPerson != null)
            {
                return BadRequest(new { message = "Email sudah terdaftar" });
            }

            bool isRegistered = _context.RegisterMurid(registerData);

            if (isRegistered)
            {
                return Ok(new { message = "Registrasi berhasil" });
            }
            return StatusCode(500, new { message = "Registrasi gagal" });
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public IActionResult login([FromBody] login loginData)
        {
            Murid murid = _context.GetMuridByEmail(loginData.Email);

            if (murid == null || murid.password != loginData.Password)
            {
                return Unauthorized(new { message = "Email atau password salah" });
            }

            var token = GenerateToken(murid);

            return Ok(new
            {
                token = token,
                user = new
                {
                    id = murid.id_murid,
                    nama = murid.nama,
                    email = murid.email,
                    role = murid.role
                }
            });
        }

        // ✅ GENERATE JWT TOKEN
        private string GenerateToken(Murid murid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, murid.id_murid.ToString()),
                new Claim(ClaimTypes.Email, murid.email),
                new Claim(ClaimTypes.Name, murid.nama),
                new Claim(ClaimTypes.Role, murid.role) // Role untuk membatasi akses
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

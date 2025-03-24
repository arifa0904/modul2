using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using modul2.Models;
using System;

namespace Modul2.Controllers
{
    [ApiController]  // ✅ Tambahkan atribut ApiController
    [Route("api/murid")]  // ✅ Set route dasar untuk API
    public class MuridController : ControllerBase  // ✅ Ubah ke ControllerBase karena ini API
    {
        private readonly MuridContext _context;

        // ✅ Gunakan Dependency Injection untuk mendapatkan connection string
        public MuridController(IConfiguration configuration)
        {
            string constr = configuration.GetConnectionString("WebApiDatabase");
            _context = new MuridContext(constr);
        }

        // ✅ Ambil semua data murid
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<IEnumerable<Murid>> GetAllMurid()
        {
            List<Murid> listMurid = _context.ListMurid();
            return Ok(listMurid);
        }

        // ✅ Tambah data murid
        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public IActionResult CreateMurid([FromBody] Murid murid)
        {
            if (murid == null)
            {
                return BadRequest("Data murid tidak boleh kosong.");
            }
            _context.RegisterMurid(murid);
            return Ok(new { message = "Data murid berhasil ditambahkan" });
        }

        // ✅ Perbarui data murid
        [HttpPut("update/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateMurid(int id, [FromBody] Murid murid)
        {
            if (murid == null)
            {
                return BadRequest("Data murid tidak boleh kosong.");
            }

            murid.id_murid = id;
            _context.UpdateMurid(murid);
            return Ok(new { message = "Data murid berhasil diperbarui" });
        }

        // ✅ Hapus data murid
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteMurid(int id)
        {
            _context.DeleteMurid(id);
            return Ok(new { message = "Data murid berhasil dihapus" });
        } 
    }
}

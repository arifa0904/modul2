using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using modul2.Models;
using System;

namespace Modul2.Controllers
{
    [ApiController]  // ✅ Tambahkan atribut ApiController
    [Route("api/[controller]")]  // ✅ Set route dasar untuk API
    public class MuridController : ControllerBase  // ✅ Ubah ke ControllerBase karena ini API
    {
        private readonly string _constr;

        // ✅ Gunakan Dependency Injection untuk mendapatkan connection string
        public MuridController(IConfiguration configuration)
        {
            string _constr = configuration.GetConnectionString("koneksi");

        }

        // ✅ Ambil semua data murid
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult GetAllMurid()
        {
            MuridContext context = new MuridContext(this._constr);
            List<Murid> listMurid = context.ListMurid();
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
            MuridContext context = new MuridContext(this._constr);
            context.AddMurid(murid);
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
            MuridContext context = new MuridContext(this._constr);
            context.UpdateMurid(murid);
            return Ok(new { message = "Data murid berhasil diperbarui" });
        }

        // ✅ Hapus data murid
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteMurid(int id)
        {
            MuridContext context = new MuridContext(this._constr);
            context.DeleteMurid(id);
            return Ok(new { message = "Data murid berhasil dihapus" });
        } 
    }
}

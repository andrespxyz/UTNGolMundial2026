using EstadisticasAPI.Data;
using EstadisticasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SedesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SedesController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sede>>> GetSedes()
            => await _context.Sedes.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Sede>> GetSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null) return NotFound();
            return sede;
        }

        [HttpPost]
        public async Task<ActionResult<Sede>> PostSede(Sede sede)
        {
            _context.Sedes.Add(sede);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSede), new { id = sede.Id }, sede);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSede(int id, Sede sede)
        {
            if (id != sede.Id) return BadRequest();
            _context.Entry(sede).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null) return NotFound();
            _context.Sedes.Remove(sede);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
using EstadisticasAPI.Data;
using EstadisticasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeleccionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeleccionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seleccion>>> GetSelecciones()
        {
            return await _context.Selecciones.OrderBy(s => s.Grupo).ThenByDescending(s => s.Puntos).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seleccion>> GetSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null) return NotFound();
            return seleccion;
        }

        [HttpGet("grupo/{grupo}")]
        public async Task<ActionResult<IEnumerable<Seleccion>>> GetPorGrupo(string grupo)
        {
            return await _context.Selecciones
                .Where(s => s.Grupo == grupo)
                .OrderByDescending(s => s.Puntos)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Seleccion>> PostSeleccion(Seleccion seleccion)
        {
            _context.Selecciones.Add(seleccion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeleccion), new { id = seleccion.Id }, seleccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeleccion(int id, Seleccion seleccion)
        {
            if (id != seleccion.Id) return BadRequest();
            _context.Entry(seleccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null) return NotFound();
            _context.Selecciones.Remove(seleccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
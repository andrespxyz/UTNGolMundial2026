using EstadisticasAPI.Data;
using EstadisticasAPI.Helpers;
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
            var selecciones = await _context.Selecciones.ToListAsync();
            return selecciones
                .OrderBy(s => s.Grupo)
                .ThenByDescending(s => s.Puntos)
                .ThenByDescending(s => s.GolesFavor - s.GolesContra)
                .ThenByDescending(s => s.GolesFavor)
                .ToList();
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
            var selecciones = await _context.Selecciones.Where(s => s.Grupo == grupo).ToListAsync();
            return selecciones
                .OrderByDescending(s => s.Puntos)
                .ThenByDescending(s => s.GolesFavor - s.GolesContra)
                .ThenByDescending(s => s.GolesFavor)
                .ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Seleccion>> PostSeleccion(Seleccion seleccion)
        {
            _context.Selecciones.Add(seleccion);
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "CREAR_SELECCION", $"Selección: {seleccion.Nombre}");
            return CreatedAtAction(nameof(GetSeleccion), new { id = seleccion.Id }, seleccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeleccion(int id, Seleccion seleccion)
        {
            var existente = await _context.Selecciones.FindAsync(id);
            if (existente == null) return NotFound();

            // Solo se actualizan los datos básicos de la selección — nunca las
            // estadísticas acumuladas (PartidosJugados, Puntos, etc.), que las
            // gestiona exclusivamente RegistrarResultado.
            existente.Nombre = seleccion.Nombre;
            existente.Codigo = seleccion.Codigo;
            existente.Grupo = seleccion.Grupo;
            existente.Escudo = seleccion.Escudo;

            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ACTUALIZAR_SELECCION", $"Selección #{id} — {existente.Nombre}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null) return NotFound();
            _context.Selecciones.Remove(seleccion);
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ELIMINAR_SELECCION", $"Selección #{id} — {seleccion.Nombre}");
            return NoContent();
        }

        [HttpGet("{id}/estadisticas")]
        public async Task<ActionResult<Seleccion>> GetEstadisticas(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null) return NotFound();
            return seleccion;
        }

        [HttpGet("grupos")]
        public async Task<ActionResult<IEnumerable<string>>> GetGrupos()
        {
            var grupos = await _context.Selecciones
                .Select(s => s.Grupo)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
            return Ok(grupos);
        }
    }
}
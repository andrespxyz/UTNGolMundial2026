using EstadisticasAPI.Data;
using EstadisticasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PartidosController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partido>>> GetPartidos()
            => await _context.Partidos
                .Include(p => p.SeleccionLocal)
                .Include(p => p.SeleccionVisitante)
                .Include(p => p.Sede)
                .OrderBy(p => p.FechaHora)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Partido>> GetPartido(int id)
        {
            var partido = await _context.Partidos
                .Include(p => p.SeleccionLocal)
                .Include(p => p.SeleccionVisitante)
                .Include(p => p.Sede)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (partido == null) return NotFound();
            return partido;
        }

        [HttpGet("grupo/{grupo}")]
        public async Task<ActionResult<IEnumerable<Partido>>> GetPorGrupo(string grupo)
            => await _context.Partidos
                .Include(p => p.SeleccionLocal)
                .Include(p => p.SeleccionVisitante)
                .Include(p => p.Sede)
                .Where(p => p.Grupo == grupo)
                .OrderBy(p => p.FechaHora)
                .ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Partido>> PostPartido(Partido partido)
        {
            _context.Partidos.Add(partido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, partido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartido(int id, Partido partido)
        {
            if (id != partido.Id) return BadRequest();
            _context.Entry(partido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/resultado")]
        public async Task<IActionResult> RegistrarResultado(int id, [FromBody] ResultadoDto resultado)
        {
            var partido = await _context.Partidos
                .Include(p => p.SeleccionLocal)
                .Include(p => p.SeleccionVisitante)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (partido == null) return NotFound();

            partido.GolesLocal = resultado.GolesLocal;
            partido.GolesVisitante = resultado.GolesVisitante;
            partido.Estado = "finalizado";

            // Actualizar estadísticas selecciones
            var local = partido.SeleccionLocal;
            var visitante = partido.SeleccionVisitante;

            local.PartidosJugados++;
            visitante.PartidosJugados++;
            local.GolesFavor += resultado.GolesLocal;
            local.GolesContra += resultado.GolesVisitante;
            visitante.GolesFavor += resultado.GolesVisitante;
            visitante.GolesContra += resultado.GolesLocal;

            if (resultado.GolesLocal > resultado.GolesVisitante)
            {
                local.PartidosGanados++; local.Puntos += 3;
                visitante.PartidosPerdidos++;
            }
            else if (resultado.GolesLocal < resultado.GolesVisitante)
            {
                visitante.PartidosGanados++; visitante.Puntos += 3;
                local.PartidosPerdidos++;
            }
            else
            {
                local.PartidosEmpatados++; local.Puntos++;
                visitante.PartidosEmpatados++; visitante.Puntos++;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null) return NotFound();
            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class ResultadoDto
    {
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
    }
}
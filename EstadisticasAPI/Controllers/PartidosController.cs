using EstadisticasAPI.Data;
using EstadisticasAPI.DTOs;
using EstadisticasAPI.Helpers;
using EstadisticasAPI.Models;
using EstadisticasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PartidoResultadoService _resultadoService;

        public PartidosController(AppDbContext context, PartidoResultadoService resultadoService)
        {
            _context = context;
            _resultadoService = resultadoService;
        }

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

        // RF09 — avance de las fases eliminatorias, agrupado y ordenado por fase.
        private static readonly string[] OrdenFasesEliminatorias =
            { "Dieciseisavos", "Octavos", "Cuartos", "Semifinal", "TercerPuesto", "Final" };

        [HttpGet("eliminatorias")]
        public async Task<ActionResult<IEnumerable<EliminatoriaFaseDto>>> GetEliminatorias()
        {
            var partidos = await _context.Partidos
                .Include(p => p.SeleccionLocal)
                .Include(p => p.SeleccionVisitante)
                .Include(p => p.Sede)
                .Where(p => OrdenFasesEliminatorias.Contains(p.Fase))
                .OrderBy(p => p.FechaHora)
                .ToListAsync();

            var resultado = OrdenFasesEliminatorias
                .Select(fase => new EliminatoriaFaseDto
                {
                    Fase = fase,
                    Partidos = partidos.Where(p => p.Fase == fase).ToList()
                })
                .Where(g => g.Partidos.Any())
                .ToList();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Partido>> PostPartido(Partido partido)
        {
            _context.Partidos.Add(partido);
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "CREAR_PARTIDO", $"Partido #{partido.Id} creado");
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, partido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartido(int id, Partido partido)
        {
            var existente = await _context.Partidos.FindAsync(id);
            if (existente == null) return NotFound();

            // Solo se actualizan los datos de calendario/emparejamiento — Estado y
            // marcador se gestionan exclusivamente por /resultado y /estado.
            existente.SeleccionLocalId = partido.SeleccionLocalId;
            existente.SeleccionVisitanteId = partido.SeleccionVisitanteId;
            existente.SedeId = partido.SedeId;
            existente.FechaHora = partido.FechaHora;
            existente.Fase = partido.Fase;
            existente.Grupo = partido.Grupo;

            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ACTUALIZAR_PARTIDO", $"Partido #{id} actualizado");
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
            if (partido.Estado == "finalizado")
                return BadRequest(new { mensaje = "El partido ya tiene un resultado registrado." });

            var resultadoReal = _resultadoService.AplicarResultado(partido, resultado);
            await _context.SaveChangesAsync();

            await _resultadoService.NotificarLiquidacionAsync(id, resultadoReal);

            await AuditoriaHelper.RegistrarAsync(_context, Request, "REGISTRAR_RESULTADO",
                $"Partido #{id} — {resultado.GolesLocal}:{resultado.GolesVisitante}");

            return NoContent();
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] EstadoDto dto)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null) return NotFound();

            partido.Estado = dto.Estado;
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "CAMBIAR_ESTADO_PARTIDO", $"Partido #{id} — nuevo estado: {dto.Estado}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null) return NotFound();
            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ELIMINAR_PARTIDO", $"Partido #{id} eliminado");
            return NoContent();
        }
    }
}
using EstadisticasAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen()
        {
            var totalSelecciones = await _context.Selecciones.CountAsync();
            var totalPartidos = await _context.Partidos.CountAsync();
            var partidosFinalizados = await _context.Partidos
                .CountAsync(p => p.Estado == "finalizado");
            var totalGoles = await _context.Partidos
                .Where(p => p.Estado == "finalizado")
                .SumAsync(p => (p.GolesLocal ?? 0) + (p.GolesVisitante ?? 0));
            var totalUsuarios = await _context.Usuarios.CountAsync(u => u.Activo);

            return Ok(new
            {
                totalSelecciones,
                totalPartidos,
                partidosFinalizados,
                partidosProgramados = totalPartidos - partidosFinalizados,
                totalGoles,
                totalUsuarios
            });
        }

        [HttpGet("goleadores")]
        public async Task<IActionResult> GetMasGoles()
        {
            var selecciones = await _context.Selecciones
                .OrderByDescending(s => s.GolesFavor)
                .Take(10)
                .Select(s => new { s.Nombre, s.Codigo, s.Grupo, s.GolesFavor, s.GolesContra, s.Puntos })
                .ToListAsync();
            return Ok(selecciones);
        }
    }
}
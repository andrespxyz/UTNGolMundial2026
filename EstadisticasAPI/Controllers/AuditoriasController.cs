using EstadisticasAPI.Data;
using EstadisticasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuditoriasController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auditoria>>> GetAuditorias()
            => await _context.Auditorias
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Auditoria>> PostAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();
            return Ok(auditoria);
        }
    }
}
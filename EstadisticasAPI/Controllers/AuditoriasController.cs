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

        // Deliberadamente sin POST: la auditoría solo se registra de forma
        // automática vía AuditoriaHelper (header X-Usuario-Id) desde los
        // propios controllers de negocio. Un POST público permitiría a
        // cualquiera insertar registros de auditoría falsos (RF24).
    }
}
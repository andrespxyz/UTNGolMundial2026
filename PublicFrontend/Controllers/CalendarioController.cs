using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class CalendarioController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;

        public CalendarioController(EstadisticasClientService estadisticas)
        {
            _estadisticas = estadisticas;
        }

        public async Task<IActionResult> Index()
        {
            var partidos = await _estadisticas.GetPartidosAsync();
            return View(partidos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var partidos = await _estadisticas.GetPartidosAsync();
            var partido = partidos.FirstOrDefault(p => p.Id == id);
            if (partido == null) return NotFound();
            return View(partido);
        }
    }
}
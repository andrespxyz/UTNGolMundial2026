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
    }
}
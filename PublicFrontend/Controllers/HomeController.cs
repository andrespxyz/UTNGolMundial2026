using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;

        public HomeController(EstadisticasClientService estadisticas)
        {
            _estadisticas = estadisticas;
        }

        public async Task<IActionResult> Index()
        {
            var selecciones = await _estadisticas.GetSeleccionesAsync();
            var partidos = await _estadisticas.GetPartidosAsync();
            ViewBag.TotalSelecciones = selecciones.Count;
            ViewBag.TotalPartidos = partidos.Count;
            ViewBag.PartidosFinalizados = partidos.Count(p => p.Estado == "finalizado");
            return View();
        }
    }
}
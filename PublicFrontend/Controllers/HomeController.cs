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
            try
            {
                var selecciones = await _estadisticas.GetSeleccionesAsync();
                var partidos = await _estadisticas.GetPartidosAsync();
                ViewBag.TotalSelecciones = selecciones.Count;
                ViewBag.TotalPartidos = partidos.Count;
                ViewBag.PartidosFinalizados = partidos.Count(p => p.Estado == "finalizado");
                ViewBag.PartidosProgramados = partidos.Count(p => p.Estado == "programado");
            }
            catch
            {
                ViewBag.TotalSelecciones = 0;
                ViewBag.TotalPartidos = 0;
                ViewBag.PartidosFinalizados = 0;
                ViewBag.PartidosProgramados = 0;
                ViewBag.Error = "No se pudo conectar con el servicio de estadísticas.";
            }
            return View();
        }
    }
}
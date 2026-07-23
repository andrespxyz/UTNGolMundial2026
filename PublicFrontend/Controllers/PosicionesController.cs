using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class PosicionesController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;

        public PosicionesController(EstadisticasClientService estadisticas)
        {
            _estadisticas = estadisticas;
        }

        public async Task<IActionResult> Index()
        {
            var selecciones = await _estadisticas.GetSeleccionesAsync();
            return View(selecciones);
        }
    }
}
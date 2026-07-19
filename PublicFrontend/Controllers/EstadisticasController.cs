using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;

        public EstadisticasController(EstadisticasClientService estadisticas)
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
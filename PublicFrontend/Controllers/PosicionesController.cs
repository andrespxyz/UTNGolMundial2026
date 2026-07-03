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
            var grupos = selecciones
                .GroupBy(s => s.Grupo)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(s => s.Puntos).ToList());
            return View(grupos);
        }
    }
}
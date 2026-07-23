using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class EliminatoriasController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;

        public EliminatoriasController(EstadisticasClientService estadisticas)
        {
            _estadisticas = estadisticas;
        }

        public async Task<IActionResult> Index()
        {
            var partidos = await _estadisticas.GetPartidosAsync();

            var fases = new[] { "Dieciseisavos", "Octavos", "Cuartos", "Semifinal", "TercerPuesto", "Final" };
            var bracket = fases.ToDictionary(
                f => f,
                f => partidos.Where(p => p.Fase == f)
                              .OrderBy(p => p.FechaHora)
                              .ToList()
            );

            return View(bracket);
        }
    }
}
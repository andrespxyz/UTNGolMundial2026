using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class PrediccionesController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;
        private readonly UTNGolCoinClientService _golcoin;

        public PrediccionesController(EstadisticasClientService estadisticas, UTNGolCoinClientService golcoin)
        {
            _estadisticas = estadisticas;
            _golcoin = golcoin;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Index", "Home");

            var partidos = await _estadisticas.GetPartidosAsync();
            var billetera = await _golcoin.GetBilleteraAsync(usuarioId.Value);
            var predicciones = billetera != null
                ? await _golcoin.GetPrediccionesAsync(billetera.Id)
                : new();

            ViewBag.Billetera = billetera;
            ViewBag.Predicciones = predicciones;

            var disponibles = partidos.Where(p =>
            {
                if (p.Estado == "finalizado") return false;
                if (DateTime.TryParse(p.FechaHora, null,
                        System.Globalization.DateTimeStyles.RoundtripKind, out var inicio))
                {
                    return DateTime.UtcNow < inicio; // oculta partidos que ya empezaron
                }
                return true; // si no se puede parsear la fecha, no lo ocultamos por eso
            }).ToList();

            return View(disponibles);
        }

        [HttpPost]
        public async Task<IActionResult> Predecir(int partidoId, string pronostico, decimal monto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Index", "Home");

            var billetera = await _golcoin.GetBilleteraAsync(usuarioId.Value);
            if (billetera == null)
            {
                TempData["Error"] = "No tienes billetera registrada.";
                return RedirectToAction("Index");
            }

            var partidos = await _estadisticas.GetPartidosAsync();
            var partido = partidos.FirstOrDefault(p => p.Id == partidoId);
            var fechaHoraPartido = partido?.FechaHora ?? "";

            var (ok, mensaje) = await _golcoin.CrearPrediccionAsync(billetera.Id, partidoId, pronostico, monto, fechaHoraPartido);
            TempData[ok ? "Exito" : "Error"] = mensaje;

            return RedirectToAction("Index");
        }
    }
}
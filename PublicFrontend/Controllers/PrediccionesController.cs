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
            return View(partidos.Where(p => p.Estado != "finalizado").ToList());
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

            var ok = await _golcoin.CrearPrediccionAsync(billetera.Id, partidoId, pronostico, monto);
            TempData[ok ? "Exito" : "Error"] = ok
                ? "¡Predicción registrada correctamente!"
                : "Error al registrar la predicción. Verifica tu saldo.";

            return RedirectToAction("Index");
        }
    }
}
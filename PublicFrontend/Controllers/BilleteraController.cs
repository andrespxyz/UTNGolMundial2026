using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;

namespace PublicFrontend.Controllers
{
    public class BilleteraController : Controller
    {
        private readonly UTNGolCoinClientService _golcoin;

        public BilleteraController(UTNGolCoinClientService golcoin)
        {
            _golcoin = golcoin;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var billetera = await _golcoin.GetBilleteraAsync(usuarioId.Value);
            if (billetera == null) return RedirectToAction("Index", "Home");

            var transacciones = await _golcoin.GetTransaccionesAsync(billetera.Id);
            var predicciones = await _golcoin.GetPrediccionesAsync(billetera.Id);

            HttpContext.Session.SetString("Saldo", billetera.Saldo.ToString("F2"));

            ViewBag.Billetera = billetera;
            ViewBag.Predicciones = predicciones;
            return View(transacciones);
        }
    }
}
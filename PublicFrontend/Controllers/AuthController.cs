using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Services;
using System.Text.Json;

namespace PublicFrontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly EstadisticasClientService _estadisticas;
        private readonly UTNGolCoinClientService _golcoin;

        public AuthController(EstadisticasClientService estadisticas, UTNGolCoinClientService golcoin)
        {
            _estadisticas = estadisticas;
            _golcoin = golcoin;
        }

        public IActionResult Login() => View();

        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var (exito, datos, mensaje) = await _estadisticas.LoginAsync(email, password);

            if (!exito)
            {
                ViewBag.Error = mensaje;
                return View();
            }

            var dict = datos as Dictionary<string, JsonElement>;
            int usuarioId = dict!["id"].GetInt32();
            string nombreUsuario = dict["nombreUsuario"].GetString()!;
            string rol = dict["rol"].GetString()!;

            HttpContext.Session.SetInt32("UsuarioId", usuarioId);
            HttpContext.Session.SetString("NombreUsuario", nombreUsuario);
            HttpContext.Session.SetString("Rol", rol);

            var billetera = await _golcoin.GetBilleteraAsync(usuarioId);
            if (billetera != null)
            {
                if (billetera.Saldo == 0)
                    await _golcoin.AplicarBonoDiarioAsync(billetera.Id);

                HttpContext.Session.SetString("Saldo", billetera.Saldo.ToString("F2"));
            }
            else
            {
                TempData["Aviso"] = "No se pudo cargar tu billetera. Verifica que UTNGolCoinAPI esté disponible.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Registro(string nombreUsuario, string email, string password)
        {
            var (exito, datos, mensaje) = await _estadisticas.RegistroAsync(nombreUsuario, email, password);

            if (!exito)
            {
                ViewBag.Error = mensaje;
                return View();
            }

            var dict = datos as Dictionary<string, JsonElement>;
            int usuarioId = dict!["id"].GetInt32();

            var (billeteraOk, billetera) = await _golcoin.CrearBilleteraAsync(usuarioId, nombreUsuario);

            HttpContext.Session.SetInt32("UsuarioId", usuarioId);
            HttpContext.Session.SetString("NombreUsuario", nombreUsuario);
            HttpContext.Session.SetString("Rol", "usuario");

            if (billeteraOk && billetera != null)
            {
                HttpContext.Session.SetString("Saldo", billetera.Saldo.ToString("F2"));
            }
            else
            {
                TempData["Aviso"] = "Tu cuenta se creó, pero no se pudo generar tu billetera UTNGolCoin. " +
                                     "Verifica que UTNGolCoinAPI (WildFly) esté corriendo e inicia sesión de nuevo.";
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
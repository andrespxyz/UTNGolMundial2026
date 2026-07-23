using Microsoft.AspNetCore.Mvc;
using PublicFrontend.Models.ViewModels;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (exito, datos, mensaje) = await _estadisticas.LoginAsync(model.Email, model.Password);

            if (!exito)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            var dict = datos as Dictionary<string, JsonElement>;
            int usuarioId = dict!["id"].GetInt32();
            string nombreUsuario = dict["nombreUsuario"].GetString()!;
            string rol = dict["rol"].GetString()!;
            string token = dict["token"].GetString()!;

            HttpContext.Session.SetInt32("UsuarioId", usuarioId);
            HttpContext.Session.SetString("NombreUsuario", nombreUsuario);
            HttpContext.Session.SetString("Rol", rol);
            HttpContext.Session.SetString("Token", token);

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
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (exito, datos, mensaje) = await _estadisticas.RegistroAsync(model.NombreUsuario, model.Email, model.Password);

            if (!exito)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            var dict = datos as Dictionary<string, JsonElement>;
            int usuarioId = dict!["id"].GetInt32();
            string token = dict["token"].GetString()!;

            // El token debe quedar en sesión ANTES de llamar a UTNGolCoinAPI:
            // CrearBilleteraAsync ahora exige un JWT válido (RF25).
            HttpContext.Session.SetInt32("UsuarioId", usuarioId);
            HttpContext.Session.SetString("NombreUsuario", model.NombreUsuario);
            HttpContext.Session.SetString("Rol", "usuario");
            HttpContext.Session.SetString("Token", token);

            var (billeteraOk, billetera) = await _golcoin.CrearBilleteraAsync(usuarioId, model.NombreUsuario);

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
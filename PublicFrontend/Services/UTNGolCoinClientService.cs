using Microsoft.AspNetCore.Http;
using PublicFrontend.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PublicFrontend.Services
{
    public class UTNGolCoinClientService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string BaseUrl = "http://localhost:8080/UTNGolCoinAPI/api";

        public UTNGolCoinClientService(IHttpContextAccessor httpContextAccessor)
        {
            _http = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }

        private JsonSerializerOptions Opts => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // RF25 — adjunta el JWT emitido por EstadisticasAPI en el login/registro,
        // guardado en sesión, para las rutas de UTNGolCoinAPI que ahora exigen
        // autenticación (todas salvo el ranking público).
        private HttpRequestMessage ConToken(HttpMethod metodo, string url)
        {
            var request = new HttpRequestMessage(metodo, url);
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return request;
        }

        public async Task<Billetera?> GetBilleteraAsync(int usuarioId)
        {
            try
            {
                var request = ConToken(HttpMethod.Get, $"{BaseUrl}/billeteras/usuario/{usuarioId}");
                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Billetera>(json, Opts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] GetBilleteraAsync falló: {ex.Message}");
                return null;
            }
        }

        public async Task<List<RankingEntry>> GetRankingAsync()
        {
            try
            {
                // Ranking público (RF21) — no requiere token.
                var json = await _http.GetStringAsync($"{BaseUrl}/billeteras/ranking");
                return JsonSerializer.Deserialize<List<RankingEntry>>(json, Opts) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] GetRankingAsync falló: {ex.Message}");
                return new();
            }
        }

        public async Task<(bool exito, string mensaje)> CrearPrediccionAsync(int billeteraId, int partidoId, string pronostico, decimal monto, string fechaHoraPartido)
        {
            try
            {
                var body = JsonSerializer.Serialize(new
                {
                    billeteraId = billeteraId.ToString(),
                    partidoId = partidoId.ToString(),
                    pronostico,
                    monto = monto.ToString(),
                    fechaHoraPartido
                });
                var request = ConToken(HttpMethod.Post, $"{BaseUrl}/predicciones");
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return (true, "¡Predicción registrada correctamente!");

                return (false, responseBody.Trim('"'));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] CrearPrediccionAsync falló: {ex.Message}");
                return (false, "No se pudo conectar con UTNGolCoinAPI.");
            }
        }

        public async Task<List<Prediccion>> GetPrediccionesAsync(int billeteraId)
        {
            try
            {
                var request = ConToken(HttpMethod.Get, $"{BaseUrl}/predicciones/billetera/{billeteraId}");
                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode) return new();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Prediccion>>(json, Opts) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] GetPrediccionesAsync falló: {ex.Message}");
                return new();
            }
        }

        public async Task<(bool exito, Billetera? billetera)> CrearBilleteraAsync(int usuarioId, string nombreUsuario)
        {
            try
            {
                var body = JsonSerializer.Serialize(new { usuarioId, nombreUsuario });
                var request = ConToken(HttpMethod.Post, $"{BaseUrl}/billeteras");
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[UTNGolCoin] CrearBilleteraAsync respondió {(int)response.StatusCode}");
                    return (false, null);
                }

                var json = await response.Content.ReadAsStringAsync();
                var billetera = JsonSerializer.Deserialize<Billetera>(json, Opts);
                return (true, billetera);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] CrearBilleteraAsync excepción: {ex.Message} — ¿WildFly/UTNGolCoinAPI está corriendo?");
                return (false, null);
            }
        }

        public async Task<bool> AplicarBonoDiarioAsync(int billeteraId)
        {
            try
            {
                var request = ConToken(HttpMethod.Post, $"{BaseUrl}/billeteras/{billeteraId}/bono-diario");
                var response = await _http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] AplicarBonoDiarioAsync falló: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Transaccion>> GetTransaccionesAsync(int billeteraId)
        {
            try
            {
                var request = ConToken(HttpMethod.Get, $"{BaseUrl}/billeteras/{billeteraId}/transacciones");
                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode) return new();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Transaccion>>(json, Opts) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] GetTransaccionesAsync falló: {ex.Message}");
                return new();
            }
        }
    }
}

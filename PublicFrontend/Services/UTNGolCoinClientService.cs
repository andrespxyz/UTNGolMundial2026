using PublicFrontend.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PublicFrontend.Services
{
    public class UTNGolCoinClientService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://localhost:8080/UTNGolCoinAPI/api";

        public UTNGolCoinClientService()
        {
            _http = new HttpClient();
        }

        private JsonSerializerOptions Opts => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public async Task<Billetera?> GetBilleteraAsync(int usuarioId)
        {
            try
            {
                var json = await _http.GetStringAsync($"{BaseUrl}/billeteras/usuario/{usuarioId}");
                return JsonSerializer.Deserialize<Billetera>(json, Opts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UTNGolCoin] GetBilleteraAsync falló: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Billetera>> GetRankingAsync()
        {
            try
            {
                var json = await _http.GetStringAsync($"{BaseUrl}/billeteras/ranking");
                return JsonSerializer.Deserialize<List<Billetera>>(json, Opts) ?? new();
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
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BaseUrl}/predicciones", content);
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
                var json = await _http.GetStringAsync($"{BaseUrl}/predicciones/billetera/{billeteraId}");
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
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BaseUrl}/billeteras", content);

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
                var response = await _http.PostAsync($"{BaseUrl}/billeteras/{billeteraId}/bono-diario", null);
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
                var json = await _http.GetStringAsync($"{BaseUrl}/billeteras/{billeteraId}/transacciones");
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
using System.Net.Http;
using System.Text;
using System.Text.Json;
using PublicFrontend.Models;

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
            catch { return null; }
        }

        public async Task<List<Billetera>> GetRankingAsync()
        {
            try
            {
                var json = await _http.GetStringAsync($"{BaseUrl}/billeteras/ranking");
                return JsonSerializer.Deserialize<List<Billetera>>(json, Opts) ?? new();
            }
            catch { return new(); }
        }

        public async Task<bool> CrearPrediccionAsync(int billeteraId, int partidoId, string pronostico, decimal monto)
        {
            try
            {
                var body = JsonSerializer.Serialize(new
                {
                    billeteraId = billeteraId.ToString(),
                    partidoId = partidoId.ToString(),
                    pronostico,
                    monto = monto.ToString()
                });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BaseUrl}/predicciones", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<Prediccion>> GetPrediccionesAsync(int billeteraId)
        {
            try
            {
                var json = await _http.GetStringAsync($"{BaseUrl}/predicciones/billetera/{billeteraId}");
                return JsonSerializer.Deserialize<List<Prediccion>>(json, Opts) ?? new();
            }
            catch { return new(); }
        }
    }
}
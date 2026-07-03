using System.Net.Http;
using System.Text.Json;
using PublicFrontend.Models;

namespace PublicFrontend.Services
{
    public class EstadisticasClientService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "https://localhost:7274/api";

        public EstadisticasClientService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };
            _http = new HttpClient(handler);
        }

        private JsonSerializerOptions Opts => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public async Task<List<Seleccion>> GetSeleccionesAsync()
        {
            var json = await _http.GetStringAsync($"{BaseUrl}/Selecciones");
            return JsonSerializer.Deserialize<List<Seleccion>>(json, Opts) ?? new();
        }

        public async Task<List<Partido>> GetPartidosAsync()
        {
            var json = await _http.GetStringAsync($"{BaseUrl}/Partidos");
            return JsonSerializer.Deserialize<List<Partido>>(json, Opts) ?? new();
        }

        public async Task<List<Sede>> GetSedesAsync()
        {
            var json = await _http.GetStringAsync($"{BaseUrl}/Sedes");
            return JsonSerializer.Deserialize<List<Sede>>(json, Opts) ?? new();
        }
    }
}
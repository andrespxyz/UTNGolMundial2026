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

        public async Task<(bool exito, object? datos, string mensaje)> LoginAsync(string email, string password)
        {
            try
            {
                var body = JsonSerializer.Serialize(new { email, password });
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BaseUrl}/Auth/login", content);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var datos = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, Opts);
                    return (true, datos, "");
                }
                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Opts);
                return (false, null, error?["mensaje"] ?? "Error al iniciar sesión.");
            }
            catch (Exception e)
            {
                return (false, null, e.Message);
            }
        }

        public async Task<(bool exito, object? datos, string mensaje)> RegistroAsync(string nombreUsuario, string email, string password)
        {
            try
            {
                var body = JsonSerializer.Serialize(new { nombreUsuario, email, password });
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BaseUrl}/Auth/registro", content);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var datos = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, Opts);
                    return (true, datos, "");
                }
                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Opts);
                return (false, null, error?["mensaje"] ?? "Error al registrarse.");
            }
            catch (Exception e)
            {
                return (false, null, e.Message);
            }
        }

    }

}
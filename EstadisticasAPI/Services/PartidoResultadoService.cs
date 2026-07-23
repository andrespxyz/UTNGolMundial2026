using EstadisticasAPI.DTOs;
using EstadisticasAPI.Models;

namespace EstadisticasAPI.Services
{
    // Encapsula la lógica de negocio de RF06/RF12/RF19: aplicar un resultado
    // oficial a las estadísticas de ambas selecciones y notificar a UTNGolCoinAPI
    // para que liquide las predicciones pendientes de ese partido.
    public class PartidoResultadoService
    {
        private readonly JwtService _jwt;

        public PartidoResultadoService(JwtService jwt)
        {
            _jwt = jwt;
        }

        // Actualiza el marcador, estado y estadísticas acumuladas de ambas
        // selecciones. Devuelve el resultado real ("LOCAL"/"EMPATE"/"VISITANTE")
        // que se necesita para notificar la liquidación.
        public string AplicarResultado(Partido partido, ResultadoDto resultado)
        {
            partido.GolesLocal = resultado.GolesLocal;
            partido.GolesVisitante = resultado.GolesVisitante;
            partido.Estado = "finalizado";

            var local = partido.SeleccionLocal!;
            var visitante = partido.SeleccionVisitante!;

            local.PartidosJugados++;
            visitante.PartidosJugados++;
            local.GolesFavor += resultado.GolesLocal;
            local.GolesContra += resultado.GolesVisitante;
            visitante.GolesFavor += resultado.GolesVisitante;
            visitante.GolesContra += resultado.GolesLocal;

            string resultadoReal;
            if (resultado.GolesLocal > resultado.GolesVisitante)
            {
                local.PartidosGanados++; local.Puntos += 3;
                visitante.PartidosPerdidos++;
                resultadoReal = "LOCAL";
            }
            else if (resultado.GolesLocal < resultado.GolesVisitante)
            {
                visitante.PartidosGanados++; visitante.Puntos += 3;
                local.PartidosPerdidos++;
                resultadoReal = "VISITANTE";
            }
            else
            {
                local.PartidosEmpatados++; local.Puntos++;
                visitante.PartidosEmpatados++; visitante.Puntos++;
                resultadoReal = "EMPATE";
            }

            return resultadoReal;
        }

        // RF12 — notifica a UTNGolCoinAPI para liquidar las predicciones del partido.
        // RF25 — /predicciones/liquidar/{id} ahora exige rol admin en UTNGolCoinAPI,
        // así que EstadisticasAPI se autentica con un token de sistema de corta duración.
        // Degradación controlada (RNF05): si UTNGolCoin no responde, el resultado
        // ya quedó guardado en EstadisticasAPI y esta falla no debe propagarse.
        public async Task NotificarLiquidacionAsync(int partidoId, string resultadoReal)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
                };
                using var http = new HttpClient(handler);
                http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwt.GenerarTokenSistema());
                var body = System.Text.Json.JsonSerializer.Serialize(new { resultadoReal });
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                await http.PostAsync($"http://localhost:8080/UTNGolCoinAPI/api/predicciones/liquidar/{partidoId}", content);
            }
            catch { /* degradación controlada: si UTNGolCoin no responde, el resultado igual se guarda */ }
        }
    }
}

using EstadisticasAPI.Data;
using EstadisticasAPI.Models;

namespace EstadisticasAPI.Helpers
{
    public static class AuditoriaHelper
    {
        // Lee el admin autor de la acción desde el header X-Usuario-Id (enviado por
        // AdminFrontend) y registra la auditoría. Si el header falta o es inválido,
        // no audita pero tampoco interrumpe la operación principal que lo llamó.
        public static async Task RegistrarAsync(AppDbContext context, HttpRequest request, string accion, string detalle)
        {
            if (!request.Headers.TryGetValue("X-Usuario-Id", out var value)) return;
            if (!int.TryParse(value, out var usuarioId) || usuarioId <= 0) return;

            try
            {
                context.Auditorias.Add(new Auditoria
                {
                    UsuarioId = usuarioId,
                    Accion = accion,
                    Detalle = detalle,
                    Fecha = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }
            catch
            {
                // degradación controlada: un fallo al auditar no debe tumbar la operación principal
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EstadisticasAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Sin [Required]: en PUT /Usuarios/{id} (actualizar rol/activo) el cliente
        // no envía este campo — solo se usa en el registro/creación de usuario.
        public string PasswordHash { get; set; } = string.Empty;

        public string Rol { get; set; } = "usuario";
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
    }
}
using System.ComponentModel.DataAnnotations;

namespace EstadisticasAPI.Models
{
    public class Sede
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Pais { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        public int Capacidad { get; set; }
    }
}
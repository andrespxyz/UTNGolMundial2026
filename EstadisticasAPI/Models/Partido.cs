using System.ComponentModel.DataAnnotations;

namespace EstadisticasAPI.Models
{
    public class Partido
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar la selección local")]
        public int SeleccionLocalId { get; set; }
        public Seleccion? SeleccionLocal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar la selección visitante")]
        public int SeleccionVisitanteId { get; set; }
        public Seleccion? SeleccionVisitante { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar la sede")]
        public int SedeId { get; set; }
        public Sede? Sede { get; set; }

        public DateTime FechaHora { get; set; }

        [Required, StringLength(30)]
        public string Fase { get; set; } = string.Empty;

        [StringLength(10)]
        public string Grupo { get; set; } = string.Empty;

        public string Estado { get; set; } = "programado";
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
    }
}
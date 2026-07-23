using System.ComponentModel.DataAnnotations;

namespace EstadisticasAPI.DTOs
{
    public class ResultadoDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Los goles no pueden ser negativos")]
        public int GolesLocal { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Los goles no pueden ser negativos")]
        public int GolesVisitante { get; set; }
    }
}

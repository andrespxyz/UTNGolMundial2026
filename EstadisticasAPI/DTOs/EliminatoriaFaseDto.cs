using EstadisticasAPI.Models;

namespace EstadisticasAPI.DTOs
{
    public class EliminatoriaFaseDto
    {
        public string Fase { get; set; } = string.Empty;
        public List<Partido> Partidos { get; set; } = new();
    }
}

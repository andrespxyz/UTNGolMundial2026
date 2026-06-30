namespace EstadisticasAPI.Models
{
    public class Seleccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Escudo { get; set; } = string.Empty;
        public int PartidosJugados { get; set; } = 0;
        public int PartidosGanados { get; set; } = 0;
        public int PartidosEmpatados { get; set; } = 0;
        public int PartidosPerdidos { get; set; } = 0;
        public int GolesFavor { get; set; } = 0;
        public int GolesContra { get; set; } = 0;
        public int Puntos { get; set; } = 0;
    }
}
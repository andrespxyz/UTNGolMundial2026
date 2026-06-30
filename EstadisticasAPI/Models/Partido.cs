namespace EstadisticasAPI.Models
{
    public class Partido
    {
        public int Id { get; set; }
        public int SeleccionLocalId { get; set; }
        public Seleccion SeleccionLocal { get; set; } = null!;
        public int SeleccionVisitanteId { get; set; }
        public Seleccion SeleccionVisitante { get; set; } = null!;
        public int SedeId { get; set; }
        public Sede Sede { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Fase { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Estado { get; set; } = "programado";
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
    }
}
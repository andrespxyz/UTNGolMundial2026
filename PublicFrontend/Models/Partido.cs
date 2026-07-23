namespace PublicFrontend.Models
{
    public class Partido
    {
        public int Id { get; set; }
        public int SeleccionLocalId { get; set; }
        public int SeleccionVisitanteId { get; set; }
        public int SedeId { get; set; }
        public string FechaHora { get; set; } = string.Empty;
        public string Fase { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
        public Seleccion? SeleccionLocal { get; set; }
        public Seleccion? SeleccionVisitante { get; set; }
        public Sede? Sede { get; set; }

        public string FechaHoraEcuador
        {
            get
            {
                if (DateTime.TryParse(FechaHora, null,
                        System.Globalization.DateTimeStyles.RoundtripKind, out var utc))
                {
                    var ecuador = utc.AddHours(-5); // Ecuador es UTC-5, sin horario de verano
                    return ecuador.ToString("dddd d 'de' MMMM, HH:mm",
                        new System.Globalization.CultureInfo("es-EC")) + " (hora Ecuador)";
                }
                return FechaHora; // si no se puede parsear, muestra el valor crudo como respaldo
            }
        }
    }
}
namespace PublicFrontend.Models
{
    public class Prediccion
    {
        public int Id { get; set; }
        public int BilleteraId { get; set; }
        public int PartidoId { get; set; }
        public string Pronostico { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal Cuota { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string FechaHora { get; set; } = string.Empty;
    }
}
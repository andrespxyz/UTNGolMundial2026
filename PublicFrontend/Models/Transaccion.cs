namespace PublicFrontend.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int BilleteraId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string FechaHora { get; set; } = string.Empty;
    }
}

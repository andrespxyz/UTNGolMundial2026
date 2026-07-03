namespace PublicFrontend.Models
{
    public class Billetera
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
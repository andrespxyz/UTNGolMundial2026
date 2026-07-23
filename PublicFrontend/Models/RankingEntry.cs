namespace PublicFrontend.Models
{
    public class RankingEntry
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public int Aciertos { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace PublicFrontend.Models.ViewModels
{
    public class PrediccionViewModel
    {
        [Required(ErrorMessage = "Debes indicar el partido")]
        public int PartidoId { get; set; }

        [Required(ErrorMessage = "Debes elegir un pronóstico")]
        [RegularExpression("LOCAL|EMPATE|VISITANTE", ErrorMessage = "El pronóstico debe ser LOCAL, EMPATE o VISITANTE")]
        public string Pronostico { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }
    }
}

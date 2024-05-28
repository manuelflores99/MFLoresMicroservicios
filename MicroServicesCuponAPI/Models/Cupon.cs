using System.ComponentModel.DataAnnotations;

namespace MicroServicesCuponAPI.Models
{
    public class Cupon
    {
        [Key]
        public int IdCupon { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public double Descuento { get; set; }
        public int CantidadMinima { get; set; }
    }
}

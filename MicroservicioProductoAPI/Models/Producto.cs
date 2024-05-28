using System.ComponentModel.DataAnnotations;

namespace MicroservicioProductoAPI.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Range(1,1000)]
        public double Precio { get; set; }
        public string Categoria { get; set; }
        public string UrlImagen { get; set; }
    }
}

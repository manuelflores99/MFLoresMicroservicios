namespace MVC.DTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string Categoria { get; set; }
        public string UrlImagen { get; set; }

        public List<DTO.ProductoDTO> Productos { get; set; }
    }
}

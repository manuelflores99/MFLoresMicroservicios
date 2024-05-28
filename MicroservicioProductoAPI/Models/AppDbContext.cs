using Microsoft.EntityFrameworkCore;

namespace MicroservicioProductoAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Producto { get; set; }
    }
}

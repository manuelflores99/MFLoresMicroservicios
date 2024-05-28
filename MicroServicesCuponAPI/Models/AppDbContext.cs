using Microsoft.EntityFrameworkCore;

namespace MicroServicesCuponAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Cupon> Cupon { get; set; }
    }
}

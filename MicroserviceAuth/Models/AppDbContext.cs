using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceAuth.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions option) : base(option) { }
    }
}

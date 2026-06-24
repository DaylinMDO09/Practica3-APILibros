using Microsoft.EntityFrameworkCore;
using APILibros.Controllers;


namespace APILibros.Modelos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<LibrosModel> Libros { get; set; }
    }
}

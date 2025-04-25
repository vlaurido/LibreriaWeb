using Microsoft.EntityFrameworkCore;
using LibreriaWeb.Models;

namespace LibreriaWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }
    }
}

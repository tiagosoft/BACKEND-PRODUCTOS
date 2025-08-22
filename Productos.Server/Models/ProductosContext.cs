using Microsoft.EntityFrameworkCore;

namespace Productos.Server.Models
{
    public class ProductosContext : DbContext
    {
        public ProductosContext(DbContextOptions<ProductosContext> options) :  base(options)
        {
        
        }
        //se crea una tabla Productos de tipo Productos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        //metodo para que el nombre d elos productos sea unico
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Producto>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}

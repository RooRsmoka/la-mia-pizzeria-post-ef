using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Models
{
    public class NuovoPizzaContext : DbContext
    {
        public DbSet<NuovaPizza> Pizze { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=pizzeriaDB;Integrated Security=True;Pooling=False");
        }
    }
}

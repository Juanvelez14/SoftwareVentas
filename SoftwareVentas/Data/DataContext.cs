using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data.Entities;

namespace SoftwareVentas.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customers> Customers { get; set; } 
        public DbSet<Product> Products { get; set; } 
    }
}

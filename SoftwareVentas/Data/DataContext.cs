using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data.Entities;

namespace SoftwareVentas.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<CustomerPhone> CustomerPhones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de las propiedades Price y Discount para Product
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("DECIMAL(18, 2)");  // Ajusta el tipo de columna para Price

            builder.Entity<Product>()
                .Property(p => p.Discount)
                .HasColumnType("DECIMAL(18, 2)");  // Ajusta el tipo de columna para Discount

            builder.Entity<Role>().Property(r => r.Id)
                .ValueGeneratedOnAdd();
            // Llamada a otras configuraciones
            ConfigureKeys(builder);
            ConfigureIndexes(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            builder.Entity<Role>().HasIndex(s => s.RoleName)
                                            .IsUnique();

            builder.Entity<Product>().HasIndex(s => s.Name)
                                             .IsUnique();

            builder.Entity<User>().HasIndex(u => u.Document)
                                            .IsUnique();
        }
    }
}

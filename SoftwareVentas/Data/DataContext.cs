using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data.Entities;
using System.Data;

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
            ConfigureKeys(builder);
            ConfigureIndexes(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            builder.Entity<RolePermission>().HasKey(rp => new {rp.RoleId, rp.PermissionId });

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

            builder.Entity<User>().HasIndex(u => u.Document)
                                            .IsUnique();
        }
    }
}

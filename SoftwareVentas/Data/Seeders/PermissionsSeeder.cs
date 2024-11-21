using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Data;
using static System.Collections.Specialized.BitVector32;

namespace SoftwareVentas.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Customers(), .. Products(), .. Users(), .. Roles()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name
                                                                        && p.Module == permission.Module);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Customers()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCustomers", Descripcion = "Ver Clientes", Module = "Customers" },
                new Permission { Name = "createCustomers", Descripcion = "Crear Clientes", Module = "Customers" },
                new Permission { Name = "editCustomers", Descripcion = "Editar Clientes", Module = "Customers" },
                new Permission { Name = "deleteCustomers", Descripcion = "Eliminar Clientes", Module = "Customers" },
            };
        }

        private List<Permission> Products()
        {
            return new List<Permission>
            {
                new Permission { Name = "showProducts", Descripcion = "Ver Productos", Module = "Products" },
                new Permission { Name = "createProducts", Descripcion = "Crear Productos", Module = "Products" },
                new Permission { Name = "editProducts", Descripcion = "Editar Productos", Module = "Products" },
                new Permission { Name = "deleteProducts", Descripcion = "Eliminar Productos", Module = "Products" },
            };
        }

        private List<Permission> Users()
        {
            return new List<Permission>
            {
                new Permission { Name = "showUsers", Descripcion = "Ver Usuarios", Module = "Users" },
                new Permission { Name = "createUsers", Descripcion = "Crear Usuarios", Module = "Users" },
                new Permission { Name = "editUsers", Descripcion = "Editar Usuarios", Module = "Users" },
                new Permission { Name = "deleteUsers", Descripcion = "Eliminar Usuarios", Module = "Users" },
            };
        }

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Name = "showRoles", Descripcion = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Descripcion = "Crear Roles", Module = "Roles" },
                new Permission { Name = "updateRoles", Descripcion = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Descripcion = "Eliminar Roles", Module = "Roles" },
            };
        }


    }
}
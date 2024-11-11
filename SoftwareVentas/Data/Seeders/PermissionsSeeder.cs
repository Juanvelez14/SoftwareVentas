using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Data.Seeders
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
            List<Permission> permissions = 
            [
                ..Products(),

                ..Customers(),

                ..Employees(),

                ..Roles(),

                ..Sales(),

                ..SalesDetails(),

                ..CustomerPhones()

                ];
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

        private List<Permission> Employees()
        {
            return new List<Permission>
            {
                new Permission { Name = "showEmployees", Descripcion = "Ver Empleados", Module = "Employees" },
                new Permission { Name = "createEmployees", Descripcion = "Crear Empleados", Module = "Employees" },
                new Permission { Name = "editEmployees", Descripcion = "Editar Empleados", Module = "Employees" },
                new Permission { Name = "deleteEmployees", Descripcion = "Eliminar Empleados", Module = "Employees" },
            };
        }

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Name = "showRoles", Descripcion = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Descripcion = "Crear Roles", Module = "Roles" },
                new Permission { Name = "editRoles", Descripcion = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Descripcion = "Eliminar Roles", Module = "Roles" },
            };
        }

        private List<Permission> Sales()
        {
            return new List<Permission>
            {
                new Permission { Name = "showSales", Descripcion = "Ver Ventas", Module = "Sales" },
                new Permission { Name = "createSales", Descripcion = "Crear Ventas", Module = "Sales" },
                new Permission { Name = "editSales", Descripcion = "Editar Ventas", Module = "Sales" },
                new Permission { Name = "deleteSales", Descripcion = "Eliminar Ventas", Module = "Sales" },
            };
        }

        private List<Permission> SalesDetails()
        {
            return new List<Permission>
            {
                new Permission { Name = "showSalesDetails", Descripcion = "Ver Detalles de Ventas", Module = "SalesDetails" },
                new Permission { Name = "createSalesDetails", Descripcion = "Crear Detalles de Ventas", Module = "SalesDetails" },
                new Permission { Name = "editSalesDetails", Descripcion = "Editar Detalles de Ventas", Module = "SalesDetails" },
                new Permission { Name = "deleteSalesDetails", Descripcion = "Eliminar Detalles de Ventas", Module = "SalesDetails" },
            };
        }

        private List<Permission> CustomerPhones()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCustomerPhones", Descripcion = "Ver Teléfonos de Clientes", Module = "CustomerPhones" },
                new Permission { Name = "createCustomerPhones", Descripcion = "Crear Teléfonos de Clientes", Module = "CustomerPhones" },
                new Permission { Name = "editCustomerPhones", Descripcion = "Editar Teléfonos de Clientes", Module = "CustomerPhones" },
                new Permission { Name = "deleteCustomerPhones", Descripcion = "Eliminar Teléfonos de Clientes", Module = "CustomerPhones" },
            };
        }
    }
}

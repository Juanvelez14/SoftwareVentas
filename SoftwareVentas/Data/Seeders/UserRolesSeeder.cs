using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Core;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Services;

namespace SoftwareVentas.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRoles();
            await CheckUsers();
        }

        private async Task CheckUsers()
        {
            // Admin
            User? user = await _usersService.GetUserAsync("usuario1@hotmail.com");

            if (user is null)
            {
                Role adminRole = _context.Roles.FirstOrDefault(r => r.RoleName == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "usuario1@hotmail.com",
                    FirstName = "Usuario",
                    LastName = "Pruebas",
                    PhoneNumber = "999999",
                    UserName = "usuario1@hotmail.com",
                    Document = "111111",
                    Role = adminRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
            // Content Manager
            user = await _usersService.GetUserAsync("usuario2@hotmail.com");

            if (user is null)
            {
                Role contentManagerRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Gestor de contenido");

                user = new User
                {
                    Email = "usuario2@hotmail.com",
                    FirstName = "Usuario2",
                    LastName = "Pruebas2",
                    PhoneNumber = "8888888",
                    UserName = "usuario2@hotmail.com",
                    Document = "222222",
                    Role = contentManagerRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }
        private async Task CheckRoles()
        {
            await AdminRoleAsync();
            await ContentManagerAsync();
            await UserManagerAsync();
        }

        private async Task UserManagerAsync()
        {
            bool exists = await _context.Roles.AnyAsync(r => r.RoleName == "Gestor de usuarios");

            if (!exists)
            {
                Role role = new Role { RoleName = "Gestor de usuarios" };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }
        private async Task ContentManagerAsync()
        {
            bool exists = await _context.Roles.AnyAsync(r => r.RoleName == "Gestor de contenido");

            if (!exists)
            {
                Role role = new Role { RoleName = "Gestor de contenido" };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.Roles.AnyAsync(r => r.RoleName == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                Role role = new Role { RoleName = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }



    }


}

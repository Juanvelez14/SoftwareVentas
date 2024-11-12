using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data; 
using SoftwareVentas.Data.Entities; 

namespace SoftwareVentas.BLL
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<IdentityResult> AssignPermissionToRoleAsync(int roleId, int permissionId);
        Task<IdentityResult> RemovePermissionFromRoleAsync(int roleId, int permissionId);
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
        Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // Método para obtener todos los roles
        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        // Método para crear un nuevo rol
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "El rol ya existe" });
            }

            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Método para eliminar un rol
        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                return await _roleManager.DeleteAsync(role);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });
        }

        // Asignar un permiso a un rol
        public async Task<IdentityResult> AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });
            }

            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Permiso no encontrado" });
            }

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        // Eliminar un permiso de un rol
        public async Task<IdentityResult> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                                               .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Relación de permiso no encontrada" });
            }

            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        // Verificar si un usuario tiene un permiso
        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            // Obtener el usuario por su ID
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            // Obtener los roles del usuario
            var userRoles = await _userManager.GetRolesAsync(user);

            // Buscar permisos asociados a los roles
            var rolesWithPermission = await _context.Roles
                                                    .Where(r => userRoles.Contains(r.RoleName))
                                                    .SelectMany(r => r.RolePermissions)
                                                    .Where(rp => rp.Permission.Name == permissionName)
                                                    .ToListAsync();

            return rolesWithPermission.Any();
        }

        // Obtener los permisos de un rol
        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var role = await _context.Roles
                                     .Include(r => r.RolePermissions)
                                     .ThenInclude(rp => rp.Permission)
                                     .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return new List<Permission>();
            }

            return role.RolePermissions.Select(rp => rp.Permission).ToList();
        }
    }
}

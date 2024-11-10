using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SoftwareVentas.BLL
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
    }
}

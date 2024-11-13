using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Services;
using System.Threading.Tasks;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // Asignar un rol a un usuario
    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
        if (result.Succeeded)
        {
            return Ok("Rol asignado correctamente");
        }

        return BadRequest(result.Errors);
    }

    // Eliminar un rol de un usuario
    [HttpPost]
    public async Task<IActionResult> RemoveRole(string userId, string roleName)
    {
        var result = await _roleService.RemoveRoleFromUserAsync(userId, roleName);
        if (result.Succeeded)
        {
            return Ok("Rol eliminado correctamente");
        }

        return BadRequest(result.Errors);
    }

    // Crear un rol
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _roleService.CreateRoleAsync(roleName);
        if (result.Succeeded)
        {
            return Ok("Rol creado correctamente");
        }

        return BadRequest(result.Errors);
    }

    // Eliminar un rol
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var result = await _roleService.DeleteRoleAsync(roleId);
        if (result.Succeeded)
        {
            return Ok("Rol eliminado correctamente");
        }

        return BadRequest(result.Errors);
    }

    // Asignar un permiso a un rol
    [HttpPost]
    public async Task<IActionResult> AssignPermissionToRole(string roleId, int permissionId)
    {
        var result = await _roleService.AssignPermissionToRoleAsync(roleId, permissionId);
        if (result.Succeeded)
        {
            return Ok("Permiso asignado correctamente");
        }

        return BadRequest(result.Errors);
    }

    // Eliminar un permiso de un rol
    [HttpPost]
    public async Task<IActionResult> RemovePermissionFromRole(string roleId, int permissionId)
    {
        var result = await _roleService.RemovePermissionFromRoleAsync(roleId, permissionId);
        if (result.Succeeded)
        {
            return Ok("Permiso eliminado correctamente");
        }

        return BadRequest(result.Errors);
    }
}
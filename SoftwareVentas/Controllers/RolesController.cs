using AspNetCoreHero.ToastNotification.Abstractions;
using t = Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Services;
using System.Data;
using SoftwareVentas.Core.Atributtes;

namespace SoftwareVentas.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRoleService rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerpage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest paginationRequest = new PaginationRequest
            {
                RecordsPerPage = RecordsPerpage ?? 15,
                Page = Page ?? 1,
                Filter = Filter,
            };

            Response<PaginationResponse<Role>> response = await _rolesService.GetListAsync(paginationRequest);

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            RoleDTO dto = new RoleDTO
            {
                Permissions = response.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Descripcion = p.Descripcion,
                    Module = p.Module,
                }).ToList()
             };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(RoleDTO dto)
        {
            if(!ModelState.IsValid)
            {
                _notyfService.Error("Debe Austar los errores de validacion");

                Response<IEnumerable<Permission>> permissionResponse1 = await _rolesService.GetPermissionsAsync();

                dto.Permissions = permissionResponse1.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Descripcion = p.Descripcion,
                    Module = p.Module,
                }).ToList();

                return View(dto);
            }

            Response<Role> createResponse = await _rolesService.CreateAsync(dto);

            if (createResponse.IsSuccess)
            {
                _notyfService.Error(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(createResponse.Message);

            Response<IEnumerable<Permission>> permissionResponse2 = await _rolesService.GetPermissionsAsync();

            dto.Permissions = permissionResponse2.Result.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Descripcion = p.Descripcion,
                Module = p.Module,
            }).ToList();

            return View(dto);
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(RoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
                dto.Permissions = permissionsByRoleResponse.Result.ToList();

                return View(dto);
            }

            Response<Role> editResponse = await _rolesService.EditAsync(dto);

            if (editResponse.IsSuccess)
            {
                _notyfService.Success(editResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(editResponse.Message);

            Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse2 = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
            dto.Permissions = permissionsByRoleResponse2.Result.ToList();
        

            return View(dto);
        }




    }

}

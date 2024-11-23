using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using static System.Collections.Specialized.BitVector32;

namespace SoftwareVentas.Services
{
    public interface IRoleService
    {
        public Task<Response<Role>> CreateAsync(RoleDTO dto);
        public Task<Response<Role>> EditAsync(RoleDTO dto);
        public Task<Response<PaginationResponse<Role>>> GetListAsync(PaginationRequest request);
        public Task<Response<RoleDTO>> GetOneAsync(int id);
        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);

    }

    public class RoleService : IRoleService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RoleService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Role>> CreateAsync(RoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Creación del rol
                    Role role = _converterHelper.ToRole(dto);
                    await _context.Roles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    int roleId = role.Id;

                    // Inserción de permisos
                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                    }

                    foreach (int permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ResponseHelper<Role>.MakeResponseSuccess(role, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<Role>.MakeResponseFail(ex);
                }
            }
        }

        public async Task<Response<Role>> EditAsync(RoleDTO dto)
        {
            try
            {
                if (dto.RoleName == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<Role>.MakeResponseFail($"El role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado.");
                }

                // Permisos
                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de permisos antiguos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

           
           

                // Actualización de Rol
                Role model = _converterHelper.ToRole(dto);
                _context.Roles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<Role>.MakeResponseSuccess(model, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Role>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Role>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Role> query = _context.Roles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.RoleName.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Role> list = await PagedList<Role>.ToPagedListAsync(query, request);

                PaginationResponse<Role> result = new PaginationResponse<Role>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Role>>.MakeResponseSuccess(result, "Roles obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Role>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<RoleDTO>> GetOneAsync(int id)
        {
            try
            {
                // Obtén el rol desde la base de datos
                Role? role = await _context.Roles.FirstOrDefaultAsync(s => s.Id == id);

                if (role is null)
                {
                    return ResponseHelper<RoleDTO>.MakeResponseFail("El Role con el id indicado no existe");
                }

                // Cargar permisos relacionados
                var rolePermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync();

                // Obtener todos los permisos y marcar los seleccionados
                var permissions = await _context.Permissions.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Descripcion = p.Descripcion,
                    Selected = rolePermissions.Contains(p.Id),
                }).ToListAsync();

                // Crear el DTO y devolverlo
                RoleDTO roleDTO = new RoleDTO
                {
                    Id = role.Id,
                    RoleName = role.RoleName,
                    Permissions = permissions,
                };

                return ResponseHelper<RoleDTO>.MakeResponseSuccess(roleDTO);
            }
            catch (Exception ex)
            {
                return ResponseHelper<RoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<RoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }

      

       
    }
}
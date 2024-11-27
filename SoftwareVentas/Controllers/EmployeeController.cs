using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Services;
using SoftwareVentas.Helpers;
using SoftwareVentas.Core.Atributtes;

namespace SoftwareVentas.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;

        public EmployeesController(IEmployeeService employeeService, INotyfService notifyService, ICombosHelper combosHelper)
        {
            _employeeService = employeeService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showEmployees", module: "Employees")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Core.Response<PaginationResponse<Employee>> response = await _employeeService.GetListAsync(request);

          

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createEmployees", module: "Employees")]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Nuevo Empleado";

            var dto = new EmployeeDTO
            {
                Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createEmployees", module: "Employees")]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            try
            {
                ViewData["Title"] = "Nuevo Empleado";
                dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();

                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación.");
                    return View(dto); // Asegúrate de pasar el modelo con Roles cargados
                }

                Core.Response<Employee> response = await _employeeService.CreateAsync(dto);

                if (!response.IsSuccess)
                {
                    _notifyService.Error(response.Message);
                    return View(dto); // Asegúrate de pasar el modelo con Roles cargados
                }

                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Ocurrió un error inesperado: {ex.Message}");
                dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
                return View(dto); // Asegúrate de pasar el modelo con Roles cargados
            }
        }

        [HttpGet]
        [CustomAuthorize(permission: "editEmployees", module: "Employees")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Core.Response<Employee> response = await _employeeService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            // Crear el DTO
            EmployeeDTO dto = new EmployeeDTO
            {
                Id = response.Result.Id,
                Name = response.Result.Name,
                RoleId = response.Result.RoleId  // Asignamos el RoleId existente del empleado
            };

            // Obtener los roles disponibles (esto dependerá de cómo los tengas almacenados)
            dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();  // Esta es una suposición, reemplaza con tu lógica real

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "editEmployees", module: "Employees")]
        public async Task<IActionResult> Edit(EmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación.");
                    return View(dto);
                }

                Core.Response<Employee> response = await _employeeService.EditAsync(dto);

                if (!response.IsSuccess)
                {
                    _notifyService.Error(response.Message);
                    return View(dto);
                }

                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Ocurrió un error: {ex.Message}");
                return View(dto);
            }
        }

        [HttpPost]
        [CustomAuthorize(permission: "deleteEmployees", module: "Employees")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Core.Response<Employee> response = await _employeeService.DeleteAsync(id);

                if (!response.IsSuccess)

                {
                    _notifyService.Error(response.Message);
                }
                else
                {
                    _notifyService.Success("Empleado eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Ocurrió un error: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

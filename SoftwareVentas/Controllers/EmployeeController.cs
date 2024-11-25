using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Services;

namespace SoftwareVentas.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly INotyfService _notifyService;

        public EmployeesController(IEmployeeService employeeService, INotyfService notifyService)
        {
            _employeeService = employeeService;
            _notifyService = notifyService;
        }

        [HttpGet]
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

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(new PaginationResponse<Employee>());
            }

            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación.");
                    return View(dto);
                }

                Core.Response<Employee> response = await _employeeService.CreateAsync(dto);

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

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Core.Response<Employee> response = await _employeeService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            EmployeeDTO dto = new EmployeeDTO
            {
                Id = response.Result.Id,
                Name = response.Result.Name,
                RoleId = response.Result.RoleId
            };

            return View(dto);
        }

        [HttpPost]
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Core.Response<bool> response = await _employeeService.DeleteAsync(id);

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

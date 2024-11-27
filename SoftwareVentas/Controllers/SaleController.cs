using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Atributtes;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using SoftwareVentas.Services;

namespace SoftwareVentas.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;

        public SalesController(ISaleService saleService, INotyfService notifyService, ICombosHelper combosHelper)
        {
            _saleService = saleService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
        }

        // Acción para listar las ventas con paginación y filtros
        [HttpGet]
        [CustomAuthorize(permission: "showSales", module: "Sales")]

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

            Core.Response<PaginationResponse<Sale>> response = await _saleService.GetListAsync(request);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(new PaginationResponse<Sale>());
            }

            return View(response.Result);
        }

        // Acción para mostrar la vista de creación de venta
        [HttpGet]
        [CustomAuthorize(permission: "createSales", module: "Sales")]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Nuevo Empleado";

            var dto = new SaleDTO
            {
                Customers = await _combosHelper.GetComboSoftwareVentasCustomersAsync(),
                Employees = await _combosHelper.GetComboSoftwareVentasEmployeesAsync()
            };

            return View(dto);
        }

        // Acción para crear una nueva venta
        [HttpPost]
        [CustomAuthorize(permission: "createSales", module: "Sales")]
        public async Task<IActionResult> Create(SaleForCreationDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación.");
                    return View(dto);
                }

                Core.Response<Sale> response = await _saleService.CreateAsync(dto);

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

        // Acción para mostrar la vista de edición de venta
        [HttpGet]
        [CustomAuthorize(permission: "editSales", module: "Sales")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Core.Response<Sale> response = await _saleService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            SaleDTO dto = new SaleDTO
            {
                Id = response.Result.Id,
                SaleDate = response.Result.SaleDate,
                CustomerId = response.Result.CustomerId,
                EmployeeId = response.Result.EmployeeId
            };

            dto.Customers = await _combosHelper.GetComboSoftwareVentasCustomersAsync();
            dto.Employees = await _combosHelper.GetComboSoftwareVentasEmployeesAsync();

            return View(dto);
        }

        // Acción para editar una venta existente
        [HttpPost]
        [CustomAuthorize(permission: "editSales", module: "Sales")]
        public async Task<IActionResult> Edit(SaleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación.");
                    return View(dto);
                }

                Core.Response<Sale> response = await _saleService.EditAsync(dto);

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

        // Acción para eliminar una venta
        [HttpPost]
        [CustomAuthorize(permission: "deleteSales", module: "Sales")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Core.Response<bool> response = await _saleService.DeleteAsync(id);

                if (!response.IsSuccess)
                {
                    _notifyService.Error(response.Message);
                }
                else
                {
                    _notifyService.Success("Venta eliminada correctamente.");
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

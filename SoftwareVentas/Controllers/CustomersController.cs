using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Atributtes;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using SoftwareVentas.Services;

// Here we define the controller
namespace SoftwareVentas.Controllers
{
	
	public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INotyfService _notifyService;
        private readonly IConverterHelper _converterHelper;

        public CustomersController(ICustomerService customerService, INotyfService notifyService, IConverterHelper converterHelper)
        {
            _customerService = customerService;
            _notifyService = notifyService;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showCustomers", module: "Customers")]
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

            Core.Response<PaginationResponse<Customer>> response = await _customerService.GetListAsync(request);


            return View(response.Result);
        }

        [CustomAuthorize(permission: "createCustomers", module: "Customers")]
        [HttpGet]
		public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [CustomAuthorize(permission: "createCustomers", module: "Customers")]
        public async Task<IActionResult> Create(CustomerDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(dto);
                }

                Core.Response<Customer> response = await _customerService.CreateAsync(dto);

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
                return View(dto);
            }
        }

		[HttpGet]
        [CustomAuthorize(permission: "editCustomers", module: "Customers")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Core.Response<Customer> response = await _customerService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [CustomAuthorize(permission: "editCustomers", module: "Customers")]
        public async Task<IActionResult> Edit(CustomerDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(dto);
                }

                Response<Customer> response = await _customerService.EditAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(dto);
            }
        }
    }
}

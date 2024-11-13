using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.Services;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Helpers;
using SoftwareVentas.Requests;

// Here we define the controller
namespace SoftwareVentas.Controllers
{
	[Authorize]
	public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INotyfService _notifyService;

        public CustomersController(ICustomerService customerService, INotyfService notifyService)
        {
            _customerService = customerService;
            _notifyService = notifyService;
        }

		[Authorize]
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

            Response<PaginationResponse<Customer>> response = await _customerService.GetListAsync(request);


            return View(response.Result);
        }

		[Authorize]
		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(customer);
                }

                Response<Customer> response = await _customerService.CreateAsync(customer);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                return View(customer);
            }
        }

		[Authorize]
		public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Customer> response = await _customerService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(customer);
                }

                Response<Customer> response = await _customerService.EditAsync(customer);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(customer);
            }
        }

		[Authorize]
		public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Customer> response = await _customerService.DeleteteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }
            else
            {
                _notifyService.Error(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int ProductId, bool Hide)
        {
            ToggleProductStatusRequest request = new ToggleProductStatusRequest
            {
                Hide = Hide,
                ProductId = ProductId
            };

            Response<Customer> response = await _customerService.ToggleAsync(request);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }
            else
            {
                _notifyService.Error(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

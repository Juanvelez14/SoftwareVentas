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

namespace SoftwareVentas.Controllers
{
    // Controlador de productos, accesible solo para usuarios autenticados
    [Authorize(Policy = "AdminOnly")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productService;
        private readonly INotyfService _notifyService;

        public ProductsController(IProductsService productService, INotyfService notifyService)
        {
            _productService = productService;
            _notifyService = notifyService;
        }

        // Esta acción ahora es accesible para todos los usuarios autenticados (sin roles específicos)
        [AllowAnonymous]  // Acceso público para ver la lista de productos
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

            Response<PaginationResponse<Product>> response = await _productService.GetListAsync(request);

            return View(response.Result);
        }

        // Acción para crear un producto, accesible solo por administradores
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(product);
                }

                Response<Product> response = await _productService.CreateAsync(product);

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
                return View(product);
            }
        }

        // Acción para editar un producto, accesible solo por administradores
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Product> response = await _productService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(product);
                }

                Response<Product> response = await _productService.EditAsync(product);

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
                return View(product);
            }
        }

        // Acción para eliminar un producto, accesible solo por administradores
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Product> response = await _productService.DeleteteAsync(id);

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

        // Acción para cambiar el estado del producto (ocultar/mostrar)
        [HttpPost]
        public async Task<IActionResult> Toggle(int ProductId, bool Hide)
        {
            ToggleProductStatusRequest request = new ToggleProductStatusRequest
            {
                Hide = Hide,
                ProductId = ProductId
            };

            Response<Product> response = await _productService.ToggleAsync(request);

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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareVentas.BLL;
using SoftwareVentas.Data.Entities;

namespace SoftwareVentas.Presentation.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // Ver la lista de productos, accesible para empleados y administradores
        [Authorize(Policy = "EmployeeOnly")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // Agregar un producto, accesible solo para administradores
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
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // Editar un producto, accesible solo para administradores
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // Eliminar un producto, accesible solo para administradores
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

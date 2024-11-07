using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.BLL;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;

// Here we define the controller
namespace SoftwareVentas.Presentation.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerService _customerService;

        // Inyección de dependencias del servicio CustomerService
        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // obtener todos los clientes
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers); 
        }

        // obtener un cliente por ID
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer); 
        }

        // agregar un cliente
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // editar un cliente
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.idCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // eliminar un cliente
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

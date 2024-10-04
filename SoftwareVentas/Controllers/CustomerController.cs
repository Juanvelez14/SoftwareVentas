using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;

// Here we define the controller
namespace SoftwareVentas.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DataContext _context;

        public CustomersController(DataContext context)
        {
            _context = context;
        }

        // Here we do the read method
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Customers> customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        // Here we make the form to create a new client
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Here we check if the model is valid
        [HttpPost]
        public async Task<IActionResult> Create(Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Here we look for the client id so we can move on to the edit view
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        //Here we verify that it is valid to be able to perform the update
        [HttpPost]
        public async Task<IActionResult> Edit(Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Here what we do is find a client by the id and delete it
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

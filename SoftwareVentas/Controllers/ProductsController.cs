using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;


// Here we define the controller
namespace SoftwareVentas.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // Here we get the list of all products in the database
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }


        // displays a form to create a product
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Check if the created product is valid
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return View(product);

                }
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
                //return RedirectToAction(nameof(Index));
            }

        }

        //Here we look for the client id so we can move on to the edit view
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(a => a.idProduct == id);


                return View(product);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
                //return RedirectToAction(nameof(Index));
            }
        }

        //Here we verify that it is valid to be able to perform the update
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                _context.Products.Update(product);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                //throw;
                return RedirectToAction(nameof(Index));
            }
        }



        // Here you find a product by id and delete it
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(a => a.idProduct == id);

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

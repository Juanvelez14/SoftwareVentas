using SoftwareVentas.BLL;

using SoftwareVentas.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareVentas.Presentation.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            return View();
        }
    }
}

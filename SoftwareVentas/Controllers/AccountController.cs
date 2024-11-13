using AspNetCoreHero.ToastNotification.Abstractions;
using SoftwareVentas.Services;
using SoftwareVentas.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareVentas.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUsersService _usersService;

		public AccountController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            // Imprime en la consola los valores recibidos
            Console.WriteLine($"Email: {dto.Email}, Password: {dto.Password}");

            if (ModelState.IsValid)
            {
                // Agregar más detalles de depuración
                var result = await _usersService.LoginAsync(dto);

                if (result.Succeeded)
                {
                    // Almacenar la sesión o cookies si es necesario
                    return RedirectToAction("Index", "Home"); // Redirige a la página de inicio
                }
                else
                {
                    // Proporcionar un mensaje más detallado sobre el error
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                    return View(dto);
                }
            }
            return View(dto);
        }


        [HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _usersService.LogoutAsync();
			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public IActionResult NotAuthorized()
		{
			return View();
		}
	}
}
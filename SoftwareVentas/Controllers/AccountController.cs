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
            if (ModelState.IsValid)
            {
                // Establecer un punto de interrupción aquí
                System.Diagnostics.Debugger.Break();

                Microsoft.AspNetCore.Identity.SignInResult result = await _usersService.LoginAsync(dto);

                // Verifica si el login fue exitoso
                if (result.Succeeded)
                {
                    // Establecer un punto de interrupción para verificar los resultados de la autenticación
                    System.Diagnostics.Debugger.Break();

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");

                // Establecer un punto de interrupción en caso de error de autenticación
                System.Diagnostics.Debugger.Break();

                return View(dto);
            }

            // Establecer un punto de interrupción si ModelState no es válido
            System.Diagnostics.Debugger.Break();

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
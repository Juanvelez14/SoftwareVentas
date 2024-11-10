using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.BLL;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Presentation.Models;

namespace SoftwareVentas.Presentation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IRoleService _roleService;

        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Obtener todos los usuarios
            List<User> users = await _usersService.GetAllUsersAsync();

            // Pasar la lista de usuarios a la vista
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener la lista de roles directamente desde el servicio UserManager o RoleManager
            var roles = await _roleService.GetAllRolesAsync();

            // Crear el DTO y asignar los roles
            UserDTO dto = new UserDTO
            {
                Role = roles.Select(r => new SelectListItem
                {
                    Value = r.Id, // Id del rol
                    Text = r.Name // Nombre del rol
                }).ToList()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, regresa a la vista con los errores
                dto.Role = (await _roleService.GetAllRolesAsync()).Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList();

                return View(dto);
            }

            // Crea un nuevo objeto User a partir de los datos del DTO
            User user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Document = dto.Document,
                RoleId = dto.RoleId // Asumiendo que en el DTO seleccionaste el RoleId
            };

            // Intenta agregar el usuario utilizando el servicio de usuarios
            IdentityResult result = await _usersService.AddUserAsync(user, dto.Password);
            
            if (result.Succeeded)
            {
                // Si el usuario se creó correctamente, redirige a la página de índice
                return RedirectToAction("/Home/Index");
            }

            // Si no se pudo crear el usuario, agrega los errores al ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Si falla, vuelve a cargar los roles para mostrarlos en el formulario de nuevo
            dto.Role = (await _roleService.GetAllRolesAsync()).Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            // Regresa a la vista con el modelo y los errores
            return View(dto);
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Services;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Models;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;

namespace SoftwareVentas.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUsersService _usersService;
		private readonly IRoleService _roleService;

        public UserController(IUsersService usersService, IRoleService roleService)
        {
            _usersService = usersService;
            _roleService = roleService;  // Inyecta el servicio _roleService
        }

        [AllowAnonymous]
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

            Core.Response<PaginationResponse<User>> response = await _usersService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleService.GetAllRolesAsync();

            if (roles == null || !roles.Any())
            {
                // Manejar el caso de roles vacíos o nulos
                ModelState.AddModelError(string.Empty, "No se encontraron roles.");
                return View(new UserDTO());  // Regresar una vista con un DTO vacío o con valores por defecto
            }

            UserDTO dto = new UserDTO
            {
                Role = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList()
            };

            return View(dto);  // Asegúrate de que siempre se regrese un valor
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
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			if (Guid.Empty.Equals(id))
			{
				return NotFound();
			}
			// Obtener el usuario por su ID
			User user = await _usersService.GetUserAsync(id.ToString());
			if (user == null)
			{
				return NotFound();
			}
			// Obtener la lista de roles
			var roles = await _roleService.GetAllRolesAsync();
			// Crear el DTO manualmente, asignando propiedades del usuario
			UserDTO dto = new UserDTO
			{
				Id = id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Document = user.Document,
				PhoneNumber = user.PhoneNumber,
				RoleId = user.RoleId,
				Role = roles.Select(r => new SelectListItem
				{
					Value = r.Id,
					Text = r.Name,
					Selected = r.Id == user.RoleId.ToString()
				}).ToList()
			};
			// Retornar el DTO a la vista
			return View(dto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UserDTO dto)
		{
			if (!ModelState.IsValid)
			{
				// Si el modelo no es válido, recarga los roles y vuelve a la vista con los errores
				var roles = await _roleService.GetAllRolesAsync();
				dto.Role = roles.Select(r => new SelectListItem
				{
					Value = r.Id,
					Text = r.Name,
					Selected = r.Id == dto.RoleId.ToString()
				}).ToList();
				return View(dto);
			}
			// Obtener el usuario existente
			User user = await _usersService.GetUserAsync(dto.Id.ToString());
			if (user == null)
			{
				return NotFound();
			}
			// Actualizar propiedades del usuario con los valores del DTO
			user.Email = dto.Email;
			user.FirstName = dto.FirstName;
			user.LastName = dto.LastName;
			user.Document = dto.Document;
			user.PhoneNumber = dto.PhoneNumber;
			user.RoleId = dto.RoleId;
			// Intentar actualizar el usuario utilizando el servicio de usuarios
			IdentityResult result = await _usersService.UpdateUserAsync(user);
			if (result.Succeeded)
			{
				// Si el usuario se actualizó correctamente, redirige a la página de índice
				return RedirectToAction("Index");
			}
			// Si la actualización falló, agrega los errores al ModelState
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			// Recarga la lista de roles en caso de error
			var rolesList = await _roleService.GetAllRolesAsync();
			dto.Role = rolesList.Select(r => new SelectListItem
			{
				Value = r.Id,
				Text = r.Name,
				Selected = r.Id == dto.RoleId.ToString()
			}).ToList();
			// Regresa a la vista con el modelo y los errores
			return View(dto);
		}
	}
}

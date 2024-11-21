using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using SoftwareVentas.Services;
using System.Data;

namespace SoftwareVentas.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUsersService _usersService;
		private readonly IRoleService _roleService;
        private readonly INotyfService _notifyService;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        public UserController(IUsersService usersService, IRoleService roleService, INotyfService notifyService, IConverterHelper converterHelper, ICombosHelper combosHelper)
        {
            _usersService = usersService;
            _roleService = roleService;  // Inyecta el servicio _roleService
            _notifyService = notifyService;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
        }

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
            UserDTO dto = new UserDTO
            {
                Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync(),
            };

            return View(dto);
        }

        [HttpPost]
		public async Task<IActionResult> Create(UserDTO dto)
		{
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
                    return View(dto);
                }

                Response<User> response = await _usersService.CreateAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
                return View(dto);
            }

        }
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _usersService.GetUserAsync(id);

            if (user is null)
            {
                return NotFound();
            }
            UserDTO dto = await _converterHelper.ToUserDTOAsync(user, false);

            return View(dto);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UserDTO dto)
		{
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
                return View(dto);
            }

            Response<User> response = await _usersService.UpdateUserAsync(dto);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            dto.Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync();
            return View(dto);
        }
	}
}

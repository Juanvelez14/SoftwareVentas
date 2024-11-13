using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Core;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace SoftwareVentas.Services
{
	public interface IUsersService
	{
		public Task<List<User>> GetAllUsersAsync();
		public Task<IdentityResult> AddUserAsync(User user, string password);
		public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
		public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<Core.Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request);
        public Task<User> GetUserAsync(string email);
		public Task<SignInResult> LoginAsync(LoginDTO dto);
		public Task LogoutAsync();
		public Task<IdentityResult> UpdateUserAsync(User user);
        Task AddToRoleAsync(User user, string roleName);
    }
	public class UserService : IUsersService
	{
		private readonly DataContext _context;
		private readonly SignInManager<User> _SignInManager;
		private readonly UserManager<User> _UserManager;

		public UserService(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
		}
		public async Task<List<User>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;


		public async Task<IdentityResult> AddUserAsync(User user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
		{
			return await _userManager.ConfirmEmailAsync(user, token);
		}

		public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
		{
			return await _userManager.GenerateEmailConfirmationTokenAsync(user);
		}

        public async Task<Core.Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<User> query = _context.Users.AsQueryable()
                                                       .Include(u => u.Role);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.FirstName.ToLower().Contains(request.Filter.ToLower())
                                            || s.LastName.ToLower().Contains(request.Filter.ToLower())
                                            || s.Document.ToLower().Contains(request.Filter.ToLower())
                                            || s.Email.ToLower().Contains(request.Filter.ToLower())
                                            || s.PhoneNumber.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<User> list = await PagedList<User>.ToPagedListAsync(query, request);

                PaginationResponse<User> result = new PaginationResponse<User>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<User>>.MakeResponseSuccess(result, "Usuarios obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<User>>.MakeResponseFail(ex);
            }
        }


        public async Task<User> GetUserAsync(string email)
		{
			User? user = await _context.Users.Include(u => u.Role)
											  .FirstOrDefaultAsync(u => u.Email == email);

			return user;
		}

		public async Task<SignInResult> LoginAsync(LoginDTO dto)
		{
			return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
		}

		public async Task LogoutAsync()
		{
			await _signInManager.SignOutAsync();
		}
		public async Task<IdentityResult> UpdateUserAsync(User user)
		{
			return await _userManager.UpdateAsync(user);
		}
		public async Task<DTOs.Response<User>> UpdateUserAsync(UserDTO dto)
		{
			try
			{
				// Buscar el usuario por su ID
				User user = await _userManager.FindByIdAsync(dto.Id.ToString());
				if (user == null)
				{
					return new DTOs.Response<User>
					{
						Success = false,
						Message = "User not found."
					};
				}
                // Actualizar las propiedades del usuario con los valores del DTO
                user.Email = dto.Email;
				user.FirstName = dto.FirstName;
				user.LastName = dto.LastName;
				user.Document = dto.Document;
				user.PhoneNumber = dto.PhoneNumber;
				user.RoleId = dto.RoleId;
				// Actualizar el usuario en la base de datos
				IdentityResult result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return new DTOs.Response<User>
					{
						Success = true,
						Data = user,
						Message = "User updated successfully."
					};
				}
				// Si falla la actualización, devolver los errores
				return new DTOs.Response<User>
				{
					Success = false,
					Message = string.Join(", ", result.Errors.Select(e => e.Description))
				};
			}
			catch (Exception ex)
			{
				// Manejar excepciones y devolver un mensaje de error
				return new DTOs.Response<User>
				{
					Success = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo asignar el rol {roleName} al usuario.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using SoftwareVentas.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.DTOs
{
	public class UserDTO
	{
		// Id como Guid en UserDTO
		public Guid Id { get; set; }

		[Display(Name = "Documento")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string Document { get; set; } = null!;

		[Display(Name = "Nombres")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string FirstName { get; set; } = null!;

		[Display(Name = "Apellido")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string LastName { get; set; } = null!;

		[Display(Name = "Telefono")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string PhoneNumber { get; set; } = null!;

		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "El campo {0} debe ser un Email valido")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "La contraseña es requerida.")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
		public string ConfirmPassword { get; set; } = null!;

		public string FullName => $"{FirstName} {LastName}";
		public string RoleId { get; set; }

		public IEnumerable<SelectListItem> Role { get; set; }
		// Conversión explícita de UserDTO a User
		public static explicit operator User(UserDTO dto)
		{
			return new User
			{
				Id = dto.Id.ToString(), // Asignación directa de Guid
				Email = dto.Email,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Document = dto.Document,
				PhoneNumber = dto.PhoneNumber
				// Asignar el resto de las propiedades necesarias
			};
		}
	}
}
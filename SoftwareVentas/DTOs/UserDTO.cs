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

		public string FullName => $"{FirstName} {LastName}";
		public int RoleId { get; set; }

		public IEnumerable<SelectListItem>? Roles { get; set; }
		// Conversión explícita de UserDTO a User
	}
}
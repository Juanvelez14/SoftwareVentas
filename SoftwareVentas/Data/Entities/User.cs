using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
	public class User : IdentityUser
	{
		[Display(Name = "Documento")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string Document { get; set; } = null!;

		[Display(Name = "Nombres")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string FirstName { get; set; } = null!;
		[Display(Name = "Apellido")]
		[MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string LastName { get; set; } = null!;

		public string FullName => $"{FirstName} {LastName}";
		// Agrega la propiedad RoleId
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string RoleId { get; set; } = null!;

		// Propiedad de navegación opcional hacia el rol
		public virtual IdentityRole Role { get; set; }
	}
}
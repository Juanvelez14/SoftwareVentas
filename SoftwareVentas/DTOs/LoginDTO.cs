using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "El campo '{0}' es requerido. ")]
		[EmailAddress(ErrorMessage = "Debe ingresar un Email valido")]

		public string Email { get; set; } = null!;

		[Display(Name = "Contraseña")]
		[MinLength(4, ErrorMessage = "El campo '{0}' debe tener al menos {1} caracteres")]
		[Required(ErrorMessage = "El campo '{0}' es requerido")]

		public string Password { get; set; } = null!;
	}
}
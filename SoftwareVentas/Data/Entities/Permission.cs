using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
	public class Permission
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Permiso")]
		[MaxLength(64, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
		[Required(ErrorMessage = "El campo '{0}' es requerido.")]
		public string Name { get; set; } = null!;

		[Display(Name = "Descripcion")]
		[MaxLength(512, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
		[Required(ErrorMessage = "El campo '{0}' es requerido.")]
		public string Descripcion { get; set; } = null!;

		[Display(Name = "Modulo")]
		[MaxLength(64, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
		[Required(ErrorMessage = "El campo '{0}' es requerido.")]
		public string Module { get; set; } = null!;

		public ICollection<RolePermission> RolePermissions { get; set; }
	}
}
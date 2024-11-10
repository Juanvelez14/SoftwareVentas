using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string RoleName { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

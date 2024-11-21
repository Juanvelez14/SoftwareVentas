using SoftwareVentas.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.DTOs
{
    public class RoleDTO
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string RoleName { get; set; } = null!;

        public List<PermissionForDTO>? Permissions {  get; set; }

        public List<sectionForDTO>? Roles { get; set; }
        public string? PermissionIds {  get; set; }
    }
}

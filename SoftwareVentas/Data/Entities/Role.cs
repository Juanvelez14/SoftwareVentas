using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class Role
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string RoleName { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; }

        public Role()
        {
            // Asignar un GUID al 'Id' si es null al crear un nuevo rol
            Id = Guid.NewGuid().ToString();
        }
    }

}

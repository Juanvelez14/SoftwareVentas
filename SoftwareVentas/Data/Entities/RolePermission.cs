using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int PermissionId { get; set; }
    }
}

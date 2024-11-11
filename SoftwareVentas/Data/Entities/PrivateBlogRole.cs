using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class PrivateBlogRole
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe terner maximo {1} caracteres.")]
        [Required(ErrorMessage = "el campo {0} es requerido.")]

        public string Name { get; set; } = null!;


        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

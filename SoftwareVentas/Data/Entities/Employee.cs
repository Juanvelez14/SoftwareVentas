using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]

        public int RoleId { get; set; }
    }
}

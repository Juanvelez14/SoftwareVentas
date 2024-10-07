using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class CustomerPhone
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int CustomerId { get; set; }

        [MaxLength(20, ErrorMessage = "El campo '{0}' debe tener un máximo de '{1}' caracteres.")]
        public string Phone { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
namespace SoftwareVentas.Data.Entities
{
    public class Customer
    {
        [Key]
        public int idCustomer { get; set; }
        [MaxLength(100, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [MaxLength(255, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        public string address { get; set; } = null!;
        [MaxLength(20, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        public string? mainPhone { get; set; } 


    }
}

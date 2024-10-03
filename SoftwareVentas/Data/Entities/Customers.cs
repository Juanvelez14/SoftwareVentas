using System.ComponentModel.DataAnnotations;
namespace SoftwareVentas.Data.Entities
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(32, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        public string Adress { get; set; } = null!;
        public int PhoneNumber { get; set; }


    }
}

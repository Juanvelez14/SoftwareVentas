using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class Product
    {
        [Key]
        public int idProduct { get; set; }
        [MaxLength(32, ErrorMessage = "El campo '{0}' debe tener maximo '{1}' caracteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int Stock { get; set; }
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100.")]
        public decimal Discount { get; set; }
    }
}

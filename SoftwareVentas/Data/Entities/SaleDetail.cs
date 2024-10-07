using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class SaleDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El campo '{0}' debe ser un número válido.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo '{0}' debe ser un precio válido.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int SaleId { get; set; }
    }
}

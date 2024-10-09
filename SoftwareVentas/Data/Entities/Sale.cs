using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int EmployeeId { get; set; }


    }
}

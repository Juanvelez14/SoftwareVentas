using Microsoft.AspNetCore.Mvc.Rendering;
using SoftwareVentas.Data.Entities;

namespace SoftwareVentas.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<SelectListItem>? Customers { get; set; }
        public int EmployeeId { get; set; }
        public IEnumerable<SelectListItem>? Employees { get; set; }
    }
    public class SaleForCreationDTO
    {
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
}

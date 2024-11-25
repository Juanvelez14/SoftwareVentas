namespace SoftwareVentas.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
    public class SaleForCreationDTO
    {
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
}

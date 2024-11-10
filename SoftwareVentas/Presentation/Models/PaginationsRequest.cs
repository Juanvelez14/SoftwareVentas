namespace SoftwareVentas.Presentation.Models
{
    public class PaginationsRequest
    {
        public int RecordsPerPage { get; set; }
        public int Page { get; set; }
        public string? Filter { get; set; }
    }
}

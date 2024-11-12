namespace SoftwareVentas.Requests
{
    public class ToggleProductStatusRequest
    {
        public int ProductId { get; set; }
        public bool Hide { get; set; }
    }
}

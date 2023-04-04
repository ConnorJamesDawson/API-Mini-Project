namespace NorthwindAPI_MiniProject.Models.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        public ICollection<OrderDetailsDTO> OrderDetails { get; set; } = new List<OrderDetailsDTO>();
    }
}

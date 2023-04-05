namespace NorthwindAPI_MiniProject.Models.DTO;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? SupplierId { get; set; }
    public int? CategoryId { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? QuantityPerUnit { get; set; }

    public List<LinkDTO> Links = new List<LinkDTO>();

}

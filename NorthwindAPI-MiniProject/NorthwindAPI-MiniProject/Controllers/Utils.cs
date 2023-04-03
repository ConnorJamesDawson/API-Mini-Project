using NorthwindAPI_MiniProject.Models.DTO;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Controllers;

public static class Utils
{
    public static ProductDTO ProductToDTO(Product product) => new ProductDTO
    {
        ProductId = product.ProductId,
        ProductName = product.ProductName,
        SupplierId = product.SupplierId,
        CategoryId = product.CategoryId,
        UnitPrice = product.UnitPrice,
    };
}

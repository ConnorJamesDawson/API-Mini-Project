using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Data.Repository;
public class ProductsRepository : NorthwindRepository<Product>
{
    public ProductsRepository(NorthwindContext context) : base(context)
    {
    }
}
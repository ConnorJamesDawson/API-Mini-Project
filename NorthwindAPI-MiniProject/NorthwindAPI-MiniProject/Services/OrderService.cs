using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Services;

public class OrderService : NorthwindService<Order>, IOrderService<Order>
{
    public OrderService(ILogger<INorthwindService<Order>> logger, INorthwindRepository<Order> respository) : base(logger, respository)
    {
    }

    public async Task<IEnumerable<Order>?> GetAllAsyncByCustomerId(string id)
    {
        if (_repository.IsNull)
        {
            return null;
        }
        return (await _repository.GetAllAsync())
                .Where(o => o.CustomerId == id.ToUpper())
                .ToList();
    }

    public async Task<Order>? GetAsyncByCustomerIdAndOrderId(string customerId, int orderId)
    {
        if (_repository.IsNull)
        {
            return null;
        }
        return (await _repository.GetAllAsync())
                .Where(o => o.CustomerId == customerId.ToUpper() && o.OrderId == orderId)
                .FirstOrDefault()!;
    }
}

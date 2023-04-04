using NorthwindAPI_MiniProject.Models;
using NuGet.Protocol.Core.Types;

namespace NorthwindAPI_MiniProject.Services
{
    public interface IOrderService<T> : INorthwindService<Order>
    {
        public Task<IEnumerable<T>?> GetAllAsyncByCustomerId(string id);
        public Task<T>? GetAsyncByCustomerIdAndOrderId(string customerId, int orderId);
    }
}

using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class OrderRepository : NorthwindRepository<Order>
    {
        public OrderRepository(NorthwindContext context) : base(context)
        {
        }
        public override async Task<Order?> FindAsync(int id)
        {
            return await _dbSet
                .Where(s => s.OrderId == id)
                .Include(s => s.OrderDetails)
                .FirstOrDefaultAsync();
        }
        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.OrderDetails)
                .ToListAsync();
        }

        public override void Remove(Order entity)
        {

            /*            foreach (var order in entity.OrderDetails)
                        {
                            entity.OrderDetails.Remove(order);
                        }*/

            entity.OrderDetails.Clear();

            base.Remove(entity);
        }
    }
}

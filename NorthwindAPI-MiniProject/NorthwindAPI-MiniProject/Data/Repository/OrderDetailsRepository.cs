using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class OrderDetailsRepository : NorthwindRepository<OrderDetail>
    {
        public OrderDetailsRepository(NorthwindContext context) : base(context)
        {
        }
        public override async Task<OrderDetail?> FindAsync(int id, int idTwo = -1)
        {
            if (idTwo == -1)
            {
                return await _dbSet
                    .Where(s => s.ProductId == id)
                    .FirstOrDefaultAsync();
            }

            var orderDetails =  _dbSet.AsNoTracking()
            .Where(od => od.OrderId == id);
            var result = await orderDetails.Where(o => o.ProductId == idTwo).FirstOrDefaultAsync();


            return result;

        }
        public override async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _dbSet
                .ToListAsync();
        }
        public override void Remove(OrderDetail entity)
        {

            base.Remove(entity);
        }
    }
}

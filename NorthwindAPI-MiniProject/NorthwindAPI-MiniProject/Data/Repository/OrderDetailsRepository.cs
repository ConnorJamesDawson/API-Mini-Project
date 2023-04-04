using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class OrderDetailsRepository : NorthwindRepository<OrderDetail>
    {
        public OrderDetailsRepository(NorthwindContext context) : base(context)
        {
        }
        public override async Task<OrderDetail?> FindAsync(int id)
        {
            return await _dbSet
                .Where(s => s.ProductId == id)
                .FirstOrDefaultAsync();
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

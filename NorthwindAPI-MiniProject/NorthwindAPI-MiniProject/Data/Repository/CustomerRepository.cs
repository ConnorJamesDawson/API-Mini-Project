using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class CustomerRepository : NorthwindRepository<Customer>, ICustomerRepository<Customer>
    {
        public CustomerRepository(NorthwindContext context) : base(context)
        {

        }

        public async Task<Customer?> FindAsync(string id)
        {
            return await _dbSet
                .Where(s => s.CustomerId == id)
                .Include(s => s.Orders)
                .ThenInclude(o => o.OrderDetails)
                .FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ToListAsync();
        }
    }
}

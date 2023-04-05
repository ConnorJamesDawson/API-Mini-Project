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
                .Include(c => c.Orders)
                .Where(c => c.CustomerId == id)
                .FirstOrDefaultAsync();
        }


    }
}

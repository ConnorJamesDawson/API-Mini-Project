namespace NorthwindAPI_MiniProject.Services
{
    public interface ICustomerService<Customer>
    {
        Task<bool> CreateAsync(Customer entity);
        Task<bool> UpdateAsync(string id, Customer entity);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<Customer>?> GetAllAsync();
        Task<Customer?> GetAsync(string id);
        Task SaveAsync();
        string CustomerIdGenerator(Customer customer);
    }
}
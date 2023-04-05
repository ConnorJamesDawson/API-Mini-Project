namespace NorthwindAPI_MiniProject.Data.Repository
{
    public interface ICustomerRepository<T> : INorthwindRepository<T> where T : class
    {
        Task<T?> FindAsync(string id);
    }
}

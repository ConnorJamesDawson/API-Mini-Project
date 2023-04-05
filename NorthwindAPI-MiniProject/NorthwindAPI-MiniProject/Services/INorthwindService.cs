namespace NorthwindAPI_MiniProject
{
    public interface INorthwindService<T>
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(int id, T entity, int productId = -1);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetAsync(int id, int idTwo = -1);
        Task SaveAsync();
    }
}

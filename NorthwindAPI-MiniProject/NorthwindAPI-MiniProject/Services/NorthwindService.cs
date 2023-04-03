using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Data.Repository;

namespace NorthwindAPI_MiniProject
{
    public class NorthwindService<T> : INorthwindService<T> where T : class
    {
        private readonly ILogger _logger;
        private readonly INorthwindRepository<T> _respository;

        public NorthwindService(ILogger<INorthwindService<T>> logger, INorthwindRepository<T> respository)
        {
            _logger = logger;
            _respository = respository;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            if (_respository.IsNull || entity == null)
            {
                return false;
            }
            else
            {
                _respository.Add(entity);
                return true;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (_respository.IsNull)
            {
                return false;
            }

            var supplier = await _respository.FindAsync(id);

            if (supplier == null)
            {
                return false;
            }

            _respository.Remove(supplier);

            await _respository.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            if (_respository.IsNull)
            {
                return null;
            }
            return (await _respository.GetAllAsync())
                .ToList();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (_respository.IsNull)
            {
                return null;
            }

            var entity = await _respository.FindAsync(id);


            if (entity == null)
            {
                _logger.LogWarning($"{typeof(T).Name} with id:{id} was not found");
                return null;
            }

            _logger.LogInformation($"{typeof(T).Name} with id:{id} was found");

            return entity;
        }

        public Task SaveAsync()
        {
            return _respository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, T entity)
        {
            if (!EntityExists(id))
            {
                return false;
            }

            _respository.Update(entity);

            try
            {
                await _respository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private bool EntityExists(int id)
        {
            return _respository.FindAsync(id).Result != null;
        }
    }
}

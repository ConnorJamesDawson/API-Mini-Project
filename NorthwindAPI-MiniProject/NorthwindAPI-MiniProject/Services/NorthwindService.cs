using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Data.Repository;

namespace NorthwindAPI_MiniProject
{
    public class NorthwindService<T> : INorthwindService<T> where T : class
    {
        protected readonly ILogger _logger;
        protected readonly INorthwindRepository<T> _repository;

        public NorthwindService(ILogger<INorthwindService<T>> logger, INorthwindRepository<T> respository)
        {
            _logger = logger;
            _repository = respository;
        }

        public async Task<bool> CreateAsync(T entity)
        {            
            if (_repository.IsNull || entity == null)
            {
                return false;
            }
            else
            {
                _repository.Add(entity);
                return true;
            }
        }

        public async Task<bool> CreateAsync(T entity, int id)
        {
            if (_repository.IsNull || entity == null)
            {
                return false;
            }
            else if (await _repository.FindAsync(id) != null)
            {
                return false;
            }
            else
            {
                _repository.Add(entity);
                return true;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (_repository.IsNull)
            {
                return false;
            }

            var supplier = await _repository.FindAsync(id);

            if (supplier == null)
            {
                return false;
            }

            _repository.Remove(supplier);

            await _repository.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            if (_repository.IsNull)
            {
                return null;
            }
            return (await _repository.GetAllAsync())
                .ToList();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (_repository.IsNull)
            {
                return null;
            }

            var entity = await _repository.FindAsync(id);


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
            return _repository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, T entity)
        {
            if (!EntityExists(id))
            {
                return false;
            }

            _repository.Update(entity);

            try
            {
                await _repository.SaveAsync();
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
            return _repository.FindAsync(id).Result != null;
        }
    }
}

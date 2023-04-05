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
            else if (await _repository.FindAsync(id, -1) != null)
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

            var supplier = await _repository.FindAsync(id, -1);

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

        public async Task<T?> GetAsync(int id, int idTwo = -1)
        {
            if (_repository.IsNull)
            {
                return null;
            }



            T entity;
            if (idTwo != -1) entity = await _repository.FindAsync(id, idTwo);
            else entity = await _repository.FindAsync(id);




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

        public async Task<bool> UpdateAsync(int id, T entity, int productId = -1)
        {
            if (!await EntityExists(id, productId))
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
                if (!await EntityExists(id, productId))
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
        private async Task<bool> EntityExists(int id, int idTwo)
        {
            if (idTwo != -1)
            {
                return (await _repository.FindAsync(id, idTwo)) != null;



            }



            return (await _repository.FindAsync(id)) != null;
        }
    }
}

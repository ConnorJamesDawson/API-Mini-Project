using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Models;
namespace NorthwindAPI_MiniProject.Services
{
    public class CustomerService : ICustomerService<Customer>
    {
        private readonly ILogger _logger;
        private readonly ICustomerRepository<Customer> _repository;



        public CustomerService(ILogger<ICustomerService<Customer>> logger, ICustomerRepository<Customer> repository)
        {
            _repository = repository;
            _logger = logger;
        }



        public async Task<bool> CreateAsync(Customer entity)
        {
            if (_repository.IsNull || entity == null)
            {
                return false;
            }
            else
            {
                _repository.Add(entity);
                _repository.SaveAsync();
                return true;
            }
        }



        public async Task<bool> DeleteAsync(string id)
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



        public async Task<IEnumerable<Customer>?> GetAllAsync()
        {
            if (_repository.IsNull)
            {
                return null;
            }
            return (await _repository.GetAllAsync())
            .ToList();
        }



        public async Task<Customer?> GetAsync(string id)
        {
            if (_repository.IsNull)
            {
                return null;
            }



            var entity = await _repository.FindAsync(id);




            if (entity == null)
            {
                _logger.LogWarning($"{typeof(Customer).Name} with id:{id} was not found");
                return null;
            }



            _logger.LogInformation($"{typeof(Customer).Name} with id:{id} was found");



            return entity;
        }



        public Task SaveAsync()
        {
            return _repository.SaveAsync();
        }



        public async Task<bool> UpdateAsync(string id, Customer entity)
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



        private bool EntityExists(string id)
        {
            return _repository.FindAsync(id).Result != null;
        }
    }
}
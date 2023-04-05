using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Models;
using System.Text.RegularExpressions;

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
                string id = CustomerIdGenerator(entity);

                entity.CustomerId = id;

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





            _repository.Update(entity);
            await _repository.SaveAsync();


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


        public string CustomerIdGenerator(Customer customer)
        {
            var customers = GetAllAsync().Result;
            var existingIds = new List<string>();
            Random rand = new Random();
            var companyNameLength = customer.CompanyName.Length;

            string generatedId;

            foreach (var cust in customers)
            {
                existingIds.Add(cust.CustomerId);
            }

            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();

            do
            {
                if(customer.CompanyName.Length > 5)
                    generatedId = customer.CompanyName.Substring(0,3).ToUpper() + Regex.Replace(guidString, "[0-9]", "").Replace("-", "").Substring(0, 2).ToUpper();
                else
                    generatedId = customer.CompanyName.Substring(0, customer.CompanyName.Length).ToUpper() + Regex.Replace(guidString, "[0-9]", "").Replace("-", "").Substring(0, 5 - customer.CompanyName.Length).ToUpper();

            } while (existingIds.Contains(generatedId));

            return generatedId;
        }
    }
}
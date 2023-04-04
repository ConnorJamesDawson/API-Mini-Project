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
            Random rand = new Random();
            var customers = GetAllAsync().Result;
            var existingIds = new List<string>();
            var companyNameLength = customer.CompanyName.Length;
            string generatedId;

            foreach (var cust in customers)
            {
                existingIds.Add(cust.CustomerId);
            }

            if(companyNameLength < 5)
            {
                generatedId = customer.CompanyName.Replace(" ", "") + customer.ContactName.Replace(" ", "").Substring(0, 5 - companyNameLength).ToUpper();
            }
            else
            {
                generatedId = customer.CompanyName.Replace(" ", "").Substring(0, 5).ToUpper();
            }

            while (existingIds.Contains(generatedId))
            {
                generatedId = customer.CompanyName.Substring(0, rand.Next(3, 5)).ToUpper() + customer.ContactName.Substring(0, rand.Next(1,3)).ToUpper();
            }

            return generatedId;
        }
    }
}
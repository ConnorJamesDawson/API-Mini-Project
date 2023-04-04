using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Services;
using NorthwindAPI_MiniProject.Models.DTO;
using static NuGet.Packaging.PackagingConstants;

namespace NorthwindAPI_MiniProject.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(ICustomerService<Customer> customerService)
        {
            _customerService = (CustomerService?)customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_customerService == null)
          {
              return NotFound();
          }
            return (await _customerService.GetAllAsync())
                .ToList();
        }

        // GET: api/Customers/VINET
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
        {

            var customer = await _customerService.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // GET: api/Customers/Orders/vinet
        [HttpGet("Orders/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByCustomerId(string customerId)
        {
            var customer = await _customerService.GetAsync(customerId);

            var orders = customer.Orders.ToList();

            if (customer == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return orders
                .Select(c => Utils.OrderToDTO(c))
                .ToList()!;
        }

        // GET: api/Customers/vinet/
        [HttpGet("/Orders/{customerId}/{OrderId}")]
        public async Task<ActionResult<OrderDTO>> GetSpecificOrderByCustomerIdThenByOrderId(string customerId, int orderId)
        {
            var customer = await _customerService.GetAsync(customerId);

            var orders = customer.Orders.ToList();

            if (customer == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return orders
                .Select(c => Utils.OrderToDTO(c))
                .Where(c => c.OrderId == orderId)
                .FirstOrDefault()!;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            await _customerService.UpdateAsync(id, customer);

            return NoContent();
        }


        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _customerService.CreateAsync(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            //var customer = await _customerService.GetAsync(id);

            //if (customer == null)
            //{
            //    return NotFound();
            //}

            // Delete the supplier
            await _customerService.DeleteAsync(id);

            return NoContent();
        }

        private bool CustomerExists(string id)
        {
            return  _customerService.GetAsync(id) != null;
        }
    }
}

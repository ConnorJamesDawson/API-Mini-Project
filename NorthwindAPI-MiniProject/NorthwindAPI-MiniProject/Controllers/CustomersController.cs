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

namespace NorthwindAPI_MiniProject.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService<Customer> _customerService;
        private readonly IOrderService<Order> _orderService;

        public CustomersController(ICustomerService<Customer> customerService, IOrderService<Order> orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
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

        // GET: api/Customers/5
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

        // GET: api/Customers/5/orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetCustomerOrders(string id)
        {
            OrdersController oc = new OrdersController(_orderService);

            return await oc.GetOrdersById(id);
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
            //string idToAssign = _customerService.CustomerIdGenerator(customer);
            //customer.CustomerId = idToAssign;

            await _customerService.CreateAsync(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteAsync(id);

            return NoContent();
        }

        private bool CustomerExists(string id)
        {
            return  _customerService.GetAsync(id) != null;
        }
    }
}

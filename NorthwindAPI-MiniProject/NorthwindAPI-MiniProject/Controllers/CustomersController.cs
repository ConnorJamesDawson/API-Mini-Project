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
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            if (customers == null)
            {
                return NotFound();
            }
            return customers.Select(c => CreateCustomerLinks(Utils.CustomerToDTO(c))).ToList();
        }

        // GET: api/Customers/5
        [HttpGet("{id}", Name = nameof(GetCustomer))]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(string id)
        {

            var customer = await _customerService.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return CreateCustomerLinks(Utils.CustomerToDTO(customer));
        }

        // GET: api/Customers/5/orders
        [HttpGet("{id}/orders", Name = nameof(GetCustomerOrders))]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetCustomerOrders(string id)
        {
            var customer = await _customerService.GetAsync(id);
            if (customer == null) return NotFound();

            return customer.Orders.Select(o => CreateOrdersLinks(Utils.OrderToDTO(o))).ToList();
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutCustomer))]
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
        public async Task<ActionResult<CustomerDTO>> PostCustomer(Customer customer)
        {
            string idToAssign = _customerService.CustomerIdGenerator(customer);
            customer.CustomerId = idToAssign;

            await _customerService.CreateAsync(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, CreateCustomerLinks(Utils.CustomerToDTO(customer)));
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}", Name = nameof(DeleteCustomer))]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteAsync(id);

            return NoContent();
        }

        private bool CustomerExists(string id)
        {
            return _customerService.GetAsync(id) != null;
        }

        private CustomerDTO CreateCustomerLinks(CustomerDTO customer)
        {
            var idObj = new { id = customer.CustomerId };

            customer.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetCustomer), idObj),
                "self",
                "GET"));
            customer.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutCustomer), idObj),
                "update_self",
                "PUT"));
            customer.Links.Add(
                new LinkDTO(Url.Link(nameof(this.DeleteCustomer), idObj),
                "delete_self",
                "DELETE"));
            customer.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetCustomerOrders), idObj),
                "orders",
                "GET"));
            return customer;
        }

        private OrderDTO CreateOrdersLinks(OrderDTO order)
        {
            if (Url == null) return order;

            var idObj = new { id = order.OrderId };

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(OrdersController.GetOrder), idObj),
                "self",
                "GET"));
            order.Links.Add(
                new LinkDTO(Url.Link(nameof(OrdersController.DeleteOrder), idObj),
                "delete_self",
                "DELETE"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(OrdersController.PutOrder), idObj),
                "update_self",
                "UPDATE"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(OrdersController.GetOrderDetails), idObj),
                "details",
                "GET"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(CustomersController.GetCustomer), new { id = order.CustomerId }),
                "owner",
                "GET"));
            return order;
        }
    }
}

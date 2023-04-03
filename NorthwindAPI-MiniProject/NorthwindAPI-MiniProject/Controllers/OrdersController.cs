using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly INorthwindService<Order> _OrderService;

        public OrdersController(INorthwindService<Order> orderService)
        {
            _OrderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _OrderService.GetAllAsync();
              if (orders == null)
              {
                  return NotFound("Cannot find orders table in the database");
              }
            return orders
                   .ToList();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _OrderService.GetAsync(id);

            if (order == null)
            {
                return NotFound("Id given does not match any order in the database.");
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, 
            [Bind("ShipAddrress", "ShipRegion", "ShipCity","ShipPostalCode", "ShipCountry")]Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest("Product given does not have a matching Id to given arguments");
            }

            if (!_OrderService.UpdateAsync(id, order).Result)
            {
                return BadRequest($"Cannot find Supplier with Id given to replace");
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderId}, order);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (order == null)
          {
                return BadRequest($"The Order given is null and has not been created.");
          }

            if (!_OrderService.CreateAsync(order).Result)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
            await _OrderService.SaveAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!OrderExists(id)) return NotFound();

            var deletedSuccessfully = await _OrderService.DeleteAsync(id);

            if (!deletedSuccessfully) return NotFound();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _OrderService.GetAsync(id).Result != null;
        }
    }
}

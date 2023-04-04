using Microsoft.AspNetCore.Mvc;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;
using NorthwindAPI_MiniProject.Services;

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
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _OrderService.GetAllAsync();
            if (orders == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return orders
                   .Select(o => Utils.OrderToDTO(o))
                   .ToList();
        }

        [HttpGet("{id}/OrderDetails")]
        public async Task<ActionResult<IEnumerable<OrderDetailsDTO>>> GetOrderDetails(int id)
        {
            var order = await _OrderService.GetAsync(id);

            if (order == null)
            {
                return NotFound("Id given does not match any order in the database.");
            }

            return order.OrderDetails
                .Select(od => Utils.OrderDetailToDTO(od))
                .ToList();
        }

        [HttpGet("{orderId}/OrderDetails/{odId}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetSpecficOrderDetails(int orderId, int odId)
        {
            var order = await _OrderService.GetAsync(orderId);

            if (order == null)
            {
                return NotFound("Id given does not match any order in the database.");
            }

            return Utils.OrderDetailToDTO(order.OrderDetails.ElementAt(odId));
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id,
            [Bind("ShipAddrress", "ShipRegion", "ShipCity", "ShipPostalCode", "ShipCountry")] Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest("Product given does not have a matching Id to given arguments");
            }

            if (!_OrderService.UpdateAsync(id, order).Result)
            {
                return BadRequest($"Cannot find Supplier with Id given to replace");
            }

            return CreatedAtAction("GetOrdersById", new { id = order.CustomerId }, order);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{orderId}/{orderDetail}")]
        public async Task<ActionResult<OrderDetailsDTO>> PostOrderDetailThroughOrderId(int orderId, OrderDetail orderDetail)
        {
            var order = await _OrderService.GetAsync(orderId);

            if (order == null)
            {
                return BadRequest($"The Order given is null and has not been created.");
            }

            order.OrderDetails.Add(orderDetail);

            if (!_OrderService.CreateAsync(order).Result)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
            await _OrderService.SaveAsync();

            return CreatedAtAction("GetOrdersById", new { id = order.CustomerId }, Utils.OrderToDTO(order));
        }

        // DELETE: api/Orders/CustomerId/OrderId
        [HttpDelete("{customerId}/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!OrderExists(orderId)) return NotFound();

            var deletedSuccessfully = await _OrderService.DeleteAsync(orderId);

            if (!deletedSuccessfully) return NotFound();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _OrderService.GetAsync(id).Result != null;
        }
    }
}

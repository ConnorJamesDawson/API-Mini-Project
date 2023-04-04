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
        private readonly INorthwindService<OrderDetail> _OrderDetailService;

        public OrdersController(INorthwindService<Order> orderService, INorthwindService<OrderDetail> orderDetailService)
        {
            _OrderService = orderService;
            _OrderDetailService = orderDetailService;
        }

        // GET: api/Orders
        [HttpGet(Name = nameof(GetOrders))]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _OrderService.GetAllAsync();
            if (orders == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return orders
                   .Select(o => CreateProductLinks(Utils.OrderToDTO(o)))
                   .ToList();
        }

        // GET: api/Orders
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _OrderService.GetAsync(id);

            if (order == null)
            {
                return NotFound("Cannot find orders table in the database");
            }

            return CreateProductLinks(Utils.OrderToDTO(order));
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
            OrderDetailsDTO orderDetailsDTO;
            try
            {
                 orderDetailsDTO = Utils.OrderDetailToDTO(order.OrderDetails.ElementAt(odId));
            }
            catch(ArgumentOutOfRangeException e)
            {
                return Problem(e.Message);
            }

            return orderDetailsDTO;
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

        [HttpPut("{OrderId}/OrderDetails/{ProductId}")]
        public async Task<IActionResult> PutOrder(int orderId, int productId,
          [Bind("UnitPrice, Quantity")] OrderDetail orderDetail)
        {
            if (orderId != orderDetail.OrderId)
            {
                return BadRequest("Product given does not have a matching Id to given arguments");
            }

            if (!_OrderDetailService.UpdateAsync(orderId, orderDetail, productId).Result)
            {
                return BadRequest($"Cannot find Supplier with Id given to replace");
            }

            return NoContent();
        }
        //POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(Order order)
        {
            if (order == null)
            {
                return BadRequest($"The Order given is null and has not been created.");
            }



            if (!_OrderService.CreateAsync(order).Result)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
            await _OrderService.SaveAsync();



            return CreatedAtAction("GetOrder", new { id = order.OrderId }, Utils.OrderToDTO(order));
        }

        // POST: api/Orders/{orderId}/{orderDetialId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{orderId}/OrderDetails")]
        public async Task<ActionResult<OrderDetailsDTO>> PostOrderDetailThroughOrderId(int orderId, OrderDetail orderDetail)
        {
            var order = await _OrderService.GetAsync(orderId);

            if (order == null)
            {
                return BadRequest($"The base Order given is null and has not been created.");
            }

            await _OrderDetailService.CreateAsync(orderDetail);

            await _OrderDetailService.SaveAsync();

            return CreatedAtAction("GetOrderDetails", new { id = orderDetail.OrderId }, Utils.OrderToDTO(order));
        }

        // DELETE: api/Orders/OrderId
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!OrderExists(orderId)) return NotFound();

            var deletedSuccessfully = await _OrderService.DeleteAsync(orderId);

            if (!deletedSuccessfully) return NotFound();

            return NoContent();
        }

        [HttpDelete("{orderId}/OrderDetails/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderId, int orderDetailId)
        {
            var order = await _OrderService.GetAsync(orderId);
            if (!OrderExists(orderId)) return NotFound();

            string key = orderId.ToString() + orderDetailId.ToString();

            var orderdetail = await _OrderDetailService.GetAsync(orderId, orderDetailId);

            order.OrderDetails.Remove(orderdetail!);

            var deletedSuccessfully = await _OrderDetailService.DeleteAsync(orderDetailId);

            if (!deletedSuccessfully) return NotFound();

            return NoContent();
        }
        private bool OrderExists(int id)
        {
            return _OrderService.GetAsync(id).Result != null;
        }
        private OrderDTO CreateProductLinks(OrderDTO order)
        {
            //if (Url == null) return product;
            var idObj = new { id = order.OrderId };

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetOrders), idObj),
                "self",
                "GET"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PostOrder), idObj),
                "post_product",
                "POST"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutOrder), idObj),
                "delete_product",
                "DELETE"));

            return order;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;
using NorthwindAPI_MiniProject.Services;

namespace NorthwindAPI_MiniProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService<Order> _OrderService;
        private readonly INorthwindService<OrderDetail> _OrderDetailService;

        public OrdersController(IOrderService<Order> orderService, INorthwindService<OrderDetail> orderDetailService)
        {
            _OrderService = orderService;
            _OrderDetailService = orderDetailService;
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
                   .Select(o => CreateOrdersLinks(Utils.OrderToDTO(o)))
                   .ToList();
        }

        // GET: api/Orders
        [HttpGet("{id}", Name = nameof(GetOrder))]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var orders = await _OrderService.GetAsync(id);
            if (orders == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return CreateOrdersLinks(Utils.OrderToDTO(orders));
        }

        // GET: api/CustomerId/Orders/vinet
        [HttpGet("{customerId}/CustomerOrders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersById(string customerId)
        {
            var orders = await _OrderService.GetAllAsyncByCustomerId(customerId);
            if (orders == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return orders
                   .Select(o => CreateOrdersLinks(Utils.OrderToDTO(o)))
                   .ToList();
        }

        [HttpGet("{id}/OrderDetails", Name = nameof(GetOrderDetails))]
        public async Task<ActionResult<IEnumerable<OrderDetailsDTO>>> GetOrderDetails(int id)
        {
            var order = await _OrderService.GetAsync(id);

            if (order == null)
            {
                return NotFound("Id given does not match any order in the database.");
            }

            return order.OrderDetails
                .Select(od => CreateOrderDetailsLinks(Utils.OrderDetailToDTO(od)))
                .ToList();
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

        // GET: api/Orders/vinet/1035
        [HttpGet("{customerId}/{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetOrdersByCustomerIdThenByOrderId(string customerId, int orderId)
        {
            var orders = await _OrderService.GetAsyncByCustomerIdAndOrderId(customerId, orderId);

            if (orders == null)
            {
                return NotFound("Cannot find orders table in the database");
            }
            return Utils.OrderToDTO(orders);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutOrder))]
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
        public async Task<ActionResult<OrderDTO>> PostOrder(Order order)
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

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, Utils.OrderToDTO(order));
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}", Name = nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!OrderExists(id)) return NotFound();

            var deletedSuccessfully = await _OrderService.DeleteAsync(id);

            if (!deletedSuccessfully) return NotFound();

            return NoContent();
        }

        [HttpDelete("{orderId}/OrderDetails/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderId, int orderDetailId)
        {
            var order = await _OrderService.GetAsync(orderId);
            if (!OrderExists(orderId)) return NotFound();

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

        private OrderDTO CreateOrdersLinks(OrderDTO order)
        {
            if (Url == null) return order;

            var idObj = new { id = order.OrderId };

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetOrder), idObj),
                "self",
                "GET"));
            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.DeleteOrder), idObj),
                "delete_self",
                "DELETE"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutOrder), idObj),
                "update_self",
                "UPDATE"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetOrderDetails), idObj),
                "details",
                "GET"));

            order.Links.Add(
                new LinkDTO(Url.Link(nameof(CustomersController.GetCustomer), new { id = order.CustomerId}),
                "owner",
                "GET"));
            return order;
        }
        private OrderDetailsDTO CreateOrderDetailsLinks(OrderDetailsDTO orderDetail)
        {
            if (Url == null) return orderDetail;

            orderDetail.Links.Add(
                new LinkDTO(Url.Link(nameof(OrdersController.GetOrder), new {id = orderDetail.OrderId}),
                "order",
                "GET"));
            orderDetail.Links.Add(
                new LinkDTO(Url.Link(nameof(ProductsController.GetProduct), new
                { id = orderDetail.ProductId}),
                "product",
                "GET"));

            return orderDetail;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthwindAPI_MiniProject.Controllers;
using NorthwindAPI_MiniProject.Models.DTO;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindAPI_MiniProject.Services;

namespace NorthwindAPI.Tests
{
    internal class OrdersControllerShould
    {
        [Category("Happy Path")]
        [Category("GetOrders")]
        [Test]
        public async Task GetOrders_WhenThereAreOrders_ReturnsListOfOrderDTOs()
        {
            var mockService = Mock.Of<IOrderService<Order>>();
            List<Order> orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1
                }
            };

            Mock
            .Get(mockService)
            .Setup(od => od.GetAllAsync().Result)
            .Returns(orders);

            var sut = new OrdersController(mockService, null);
            var result = await sut.GetOrders();
            Assert.That(result.Value, Is.InstanceOf<IEnumerable<OrderDTO>>());
        }

        [Category("Sad Path")]
        [Category("GetOrders")]
        [Test]
        public async Task GetOrders_WhenThereAreNoOrders_ReturnsNull()
        {
            var mockService = Mock.Of<IOrderService<Order>>();

            Mock
            .Get(mockService)
            .Setup(od => od.GetAllAsync().Result)
            .Returns((List<Order>)null);

            var sut = new OrdersController(mockService, null);
            var result = await sut.GetOrders();
            Assert.That(result.Value, Is.Null);
        }


        [Category("Happy Path")]
        [Category("GetOrder")]
        [Test]
        public async Task GetOrder_WhenGivenValidId_ReturnsAProductDTO()
        {
            var mockService = Mock.Of<IOrderService<Order>>();
            List<Order> orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1
                }
            };

            Mock
            .Get(mockService)
            .Setup(od => od.GetAsync(1, -1).Result)
            .Returns(orders[0]);

            var sut = new OrdersController(mockService, null);
            var result = await sut.GetOrder(1);
            Assert.That(result.Value, Is.InstanceOf<OrderDTO>());
        }

        [Category("Sad Path")]
        [Category("GetOrder")]
        [Test]
        public async Task GetOrder_WhenGivenInvalidId_ReturnsNotFoundResult()
        {
            var mockService = Mock.Of<IOrderService<Order>>();

            Mock
            .Get(mockService)
            .Setup(od => od.GetAsync(1, -1).Result)
            .Returns<Task>(null);

            var sut = new OrdersController(mockService, null);
            var result = await sut.GetOrder(1);
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Category("Happy Path")]
        [Category("DeleteOrder")]
        [Test]
        public async Task DeleteOrder_WhenGivenValidId_HitsDeleteAsyncOnceAndReturnsNoContent()
        {
            var mockService = new Mock<IOrderService<Order>>(behavior: MockBehavior.Strict);
            Order order = new Order { OrderId = 1 };

            mockService
                .Setup(cs => cs.GetAsync(1, -1).Result)
                .Returns(order);

            mockService
                .Setup(cs => cs.DeleteAsync(1))
                .ReturnsAsync(true);

            var sut = new OrdersController(mockService.Object, null);
            var result = await sut.DeleteOrder(1);

            mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Category("Happy Path")]
        [Category("PutOrder")]
        [Test]
        public async Task PutOrder_WhenGivenCorrectDetails_ReturnsUpdatedOrder()
        {
            var mockService = Mock.Of<IOrderService<Order>>();

            Order order = new Order
            {
                OrderId = 1,
                Freight = 180,
                ShipCountry = "France"
            };

            Mock
            .Get(mockService)
            .Setup(od => od.GetAsync(1, -1).Result)
            .Returns(order);

            var sut = new OrdersController(mockService, null);
            await sut.PutOrder(1, order);

            var result = await sut.GetOrder(1);

            Assert.That(result.Value.ShipCountry, Is.EqualTo(order.ShipCountry));
        }

        [Category("Happy Path")]
        [Category("PostSupplier")]
        [Test]
        public async Task PostOrder_WhenGivenCorrectDetails_AddSupplier()
        {
            var mockService = Mock.Of<IOrderService<Order>>();

            Order order = new Order
            {
                OrderId = 1,
                Freight = 180,
                ShipCountry = "France"
            };

            List<Order> suppliers = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    Freight = 180,
                    ShipCountry = "France"
                }
            };

            Mock
            .Get(mockService)
            .Setup(od => od.GetAllAsync().Result)
            .Returns(suppliers);

            var sut = new OrdersController(mockService, null);
            await sut.PostOrder(order);
            var result = await sut.GetOrders();

            Assert.That(result.Value, Is.InstanceOf<IEnumerable<OrderDTO>>());
        }
    }
}

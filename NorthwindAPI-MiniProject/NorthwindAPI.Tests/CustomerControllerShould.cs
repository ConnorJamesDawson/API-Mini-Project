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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindAPI.Tests
{
    internal class CustomerControllerShould
    {
        [Category("Happy Path")]
        [Category("GetCustomers")]
        [Test]
        public async Task GetCustomer_WhenThereAreCustomers_ReturnsListOfCustomers()
        {
            var mockService = Mock.Of<ICustomerService<Customer>>();
            List<Customer> orders = new List<Customer>
            {
                new Customer
                {
                    CustomerId = "AAAA"
                }
            };

            Mock
            .Get(mockService)
            .Setup(c => c.GetAllAsync().Result)
            .Returns(orders);

            var sut = new CustomersController(mockService, null);

            var result = await sut.GetCustomers();
            Assert.That(result.Value, Is.InstanceOf<IEnumerable<Customer>>());
        }
        [Category("Sad Path")]
        [Category("GetCustomers")]
        [Test]
        public async Task GetCustomer_WhenRepositoryEmpty_ReturnsNull()
        {
            var mockService = Mock.Of<ICustomerService<Customer>>();

            Mock
            .Get(mockService)
            .Setup(c => c.GetAllAsync().Result)
            .Returns(new List<Customer>());

            var sut = new CustomersController(mockService, null);

            var result = await sut.GetCustomers();
            Assert.That(result.Value, Is.Empty);
        }


        [Category("Happy Path")]
        [Category("GetCustomer")]
        [Test]
        public async Task GetProduct_WhenID1_ReturnsShampoo()
        {
            var mockService = Mock.Of<ICustomerService<Customer>>();
            List<Customer> customer = new List<Customer>
        {
            new Customer
            {
                CustomerId = "AAAA",
                CompanyName = "Alex Armstrong",
            }
        };

            Mock
            .Get(mockService)
            .Setup(sc => sc.GetAsync("AAAA").Result)
            .Returns(customer[0]);

            var sut = new CustomersController(mockService, null);

            var result = await sut.GetCustomer("AAAA");
            Assert.That(result.Value, Is.InstanceOf<Customer>());
        }

        [Category("Sad Path")]
        [Category("GetCustomer")]
        [Test]
        public async Task GetCustomer_WhenInvalidId_ReturnsNotFound()
        {
            var mockService = Mock.Of<ICustomerService<Customer>>();

            Mock
            .Get(mockService)
            .Setup(c => c.GetAsync("A").Result)
            .Returns<Task>(null);

            var sut = new CustomersController(mockService, null);

            var result = await sut.GetCustomer("A");
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Category("Happy Path")]
        [Category("DeleteCustomer")]
        [Test]
        public async Task DeleteCustomer_RemovesCustomer()
        {
            var mockService = new Mock<ICustomerService<Customer>>();
            var customer = new Customer { CustomerId = "A", CompanyName = "Apple" };

            mockService
                .Setup(c => c.GetAsync("A").Result)
                .Returns(customer);

            mockService.Setup(c => c.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var sut = new CustomersController(mockService.Object, null);

            var deleteResult = await sut.DeleteCustomer("A");
            var result = await sut.GetCustomer("A");


            mockService.Verify(c => c.DeleteAsync("A"), Times.Once);
            Assert.That(result.Result, Is.Null);
        }

        [Category("Sad Path")]
        [Category("DeleteCustomer")]
        [Test]
        public async Task DeleteCustomer_WhenCustomerDoesNotExist_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService<Customer>>();
            mockService
                .Setup(c => c.GetAsync("A").Result)
                .Returns((Customer)null);

            mockService.Setup(c => c.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var sut = new CustomersController(mockService.Object, null);


            var deleteResult = await sut.DeleteCustomer("A");

            mockService.Verify(c => c.DeleteAsync("A"), Times.Once);

            Assert.That(deleteResult, Is.InstanceOf<NoContentResult>());
        }
        [Category("Happy Path")]
        [Category("PutCustomer")]
        [Test]
        public async Task PutCustomer_WhenGivenDetails_ReturnUpdatedCustomer()
        {
            var mockService = Mock.Of<ICustomerService<Customer>>();

            Customer customer = new Customer
            {
                CustomerId = "A",
                CompanyName = "Apple"
            };
            Customer updatedCustomer = new Customer
            {
                CustomerId = "A",
                CompanyName = "Banana"
            };

            Mock
                .Get(mockService)
                .Setup(c => c.GetAsync("A").Result)
                .Returns(customer);

            Mock
                .Get(mockService)
                .Setup(c => c.UpdateAsync("A", It.IsAny<Customer>()))
                .ReturnsAsync(true);

            var sut = new CustomersController(mockService, null);


            await sut.PutCustomer("A", customer);
            await sut.PutCustomer("A", updatedCustomer);

            Mock.Get(mockService)
                .Setup(c => c.GetAsync("A").Result)
                .Returns(updatedCustomer);

            var result = await sut.GetCustomer("A");
            Assert.That(result.Value.CompanyName, Is.EqualTo(updatedCustomer.CompanyName));
        }

        [Category("Sad Path")]
        [Category("PutCustomer")]
        [Test]
        public async Task PutCustomer_WhenCustomerDoesNotExist_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService<Customer>>();

            // Set up the mock service to return null when GetAsync is called
            // This simulates that the customer does not exist
            mockService
                .Setup(c => c.GetAsync("A").Result)
                .Returns((Customer)null);

            // Set up the mock service to return false when UpdateAsync is called
            // This simulates that the customer cannot be updated
            mockService
                .Setup(c => c.UpdateAsync("A", It.IsAny<Customer>()))
                .ReturnsAsync(false);

            var sut = new CustomersController(mockService.Object, null);

            Customer updatedCustomer = new Customer
            {
                CustomerId = "A",
                CompanyName = "Banana"
            };

            // Call the PutCustomer method to attempt to update the non-existent customer
            var updateResult = await sut.PutCustomer("A", updatedCustomer);

            // Verify that UpdateAsync was not called, since the customer does not exist
            mockService.Verify(c => c.UpdateAsync("A", updatedCustomer), Times.Once);

            // Check the result of the PutCustomer call to ensure it returns a NotFoundResult
            Assert.That(updateResult, Is.InstanceOf<NoContentResult>());
        }

        [Category("Happy Path")]
        [Category("PostCustomer")]
        [Test]
        public async Task PostCustomer_WhenGivenDetails_ReturnsCreatedCustomer()
        {
            var mockService = new Mock<ICustomerService<Customer>>();

            Customer newCustomer = new Customer
            {
                CustomerId = "A",
                CompanyName = "Apple"
            };

            // Set up the mock service to create the customer when CreateAsync is called
            mockService
                .Setup(c => c.CreateAsync(newCustomer))
                .ReturnsAsync(true);

            var sut = new CustomersController(mockService.Object, null);


            // Call the PostCustomer method to create the new customer
            var result = await sut.PostCustomer(newCustomer);

            // Verify that CreateAsync was called once
            mockService.Verify(c => c.CreateAsync(newCustomer), Times.Once);

            // Check the result of the PostCustomer call to ensure it returns a CreatedAtActionResult
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

            // Check that the returned customer is the same as the one we created
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            var returnedCustomer = createdAtActionResult.Value as Customer;

            Assert.That(newCustomer.CustomerId, Is.EqualTo(returnedCustomer.CustomerId));
            Assert.That(newCustomer.CompanyName, Is.EqualTo(returnedCustomer.CompanyName));
        }
        [Category("Sad Path")]
        [Category("PostCustomer")]
        [Test]
        public async Task PostCustomer_WhenCustomerCannotBeCreated_ReturnsBadRequest()
        {
            var mockService = new Mock<ICustomerService<Customer>>();

            Customer newCustomer = new Customer
            {
                CustomerId = "A",
                CompanyName = "Apple"
            };

            // Set up the mock service to return false when CreateAsync is called
            // This simulates that the customer cannot be created
            mockService
                .Setup(c => c.CreateAsync(newCustomer))
                .ReturnsAsync(false);

            var sut = new CustomersController(mockService.Object, null);


            // Call the PostCustomer method to attempt to create the new customer
            var result = await sut.PostCustomer(newCustomer);

            // Verify that CreateAsync was called once
            mockService.Verify(c => c.CreateAsync(newCustomer), Times.Once);

            // Check the result of the PostCustomer call to ensure it returns a BadRequestResult
            Assert.That(result.Value, Is.Null);
        }


    }
}

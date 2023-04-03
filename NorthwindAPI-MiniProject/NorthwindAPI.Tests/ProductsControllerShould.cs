﻿using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NorthwindAPI_MiniProject;
using NorthwindAPI_MiniProject.Controllers;
using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;
using System.Security.Cryptography;

namespace NorthwindAPI.Tests;

internal class ProductsControllerShould
{
    [Category("Happy Path")]
    [Category("GetProduct")]
    [Test]
    public async Task GetProduct_WhenThereAreProducts_ReturnsListOfProductDTOs()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();
        List<Product> products = new List<Product> 
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Shampoo",
                UnitsInStock = 5
            }
        };



        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAllAsync().Result)
        .Returns(products);





        var sut = new ProductsController(mockService);
        var result = await sut.GetProducts();
        Assert.That(result.Value, Is.InstanceOf<IEnumerable<ProductDTO>>());
    }

    [Category("Sad Path")]
    [Category("GetProducts")]
    [Test]
    public async Task GetProduct_WhenRepositoryEmpty_ReturnsNull()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();
        
        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAllAsync().Result)
        .Returns((List<Product>)null);

        var sut = new ProductsController(mockService);
        var result = await sut.GetProducts();
        Assert.That(result.Value, Is.Null);
    }


    [Category("Happy Path")]
    [Category("GetProduct")]
    [Test]
    public async Task GetProduct_WhenID1_ReturnsShampoo()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();
        List<Product> products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Shampoo",
                UnitsInStock = 5
            }
        };



        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAsync(1).Result)
        .Returns(products[0]);





        var sut = new ProductsController(mockService);
        var result = await sut.GetProduct(1);
        Assert.That(result.Value, Is.InstanceOf<ProductDTO>());
    }

    [Category("Sad Path")]
    [Category("GetProduct")]
    [Test]
    public async Task GetProduct_WhenInvalidId_ReturnsNotFound()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();
       
        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAsync(1).Result)
        .Returns((Product)null);

        var sut = new ProductsController(mockService);
        var result = await sut.GetProduct(1);
        Assert.That(result.Value, Is.InstanceOf<NotFoundResult>());
    }



    [Category("Happy Path")]
    [Category("DeleteProduct")]
    [Test]
    public async Task DeleteProduct_RemovesProduct()
    {
        var mockService = new Mock<INorthwindService<Product>>();

        mockService.Setup(sc => sc.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        var sut = new ProductsController(mockService.Object);
        var result = await sut.DeleteProduct(1);

        mockService.Verify(s => s.DeleteAsync(1), Times.Once);
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Category("Happy Path")]
    [Category("PutProduct")]
    [Test]
    public async Task PutProduct_WhenGivenDetails_ReturnUpdatedProduct()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();



        Product prod = new Product
        {
            ProductId = 1,
            ProductName = "Shower Gel",
            UnitsInStock = 5
        };

        List<Product> products = new List<Product> 
        {
            new Product
                {
                    ProductId = 1,
                    ProductName = "Shampoo",
                    UnitsInStock = 5
                } 
        };

        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAsync(0).Result)
        .Returns(prod);

        var sut = new ProductsController(mockService);
        await sut.PutProduct(0, prod);

        var result = await sut.GetProduct(0);

        Assert.That(result.Value.ProductName, Is.EqualTo(prod.ProductName));
    }
    [Category("Happy Path")]
    [Category("PostSupplier")]
    [Test]
    public async Task PostSupplier_WhenGivenDetails_AddSupplier()
    {
        var mockService = Mock.Of<INorthwindService<Product>>();

        Product prod = new Product
        {
            ProductId = 1,
            ProductName = "Shampoo",
            UnitsInStock = 5
        };

        List<Product> suppliers = new List<Product> 
        {
            new Product
                {
                    ProductId = 1,
                    ProductName = "Shampoo",
                    UnitsInStock = 5
                } 
        };

        Mock
        .Get(mockService)
        .Setup(sc => sc.GetAllAsync().Result)
        .Returns(suppliers);

        var sut = new ProductsController(mockService);
        await sut.PostProduct(prod);
        var result = await sut.GetProducts();

        Assert.That(result.Value, Is.InstanceOf<IEnumerable<ProductDTO>>());
    }

}

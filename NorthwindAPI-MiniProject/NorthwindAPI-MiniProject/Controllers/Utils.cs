using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;

namespace NorthwindAPI_MiniProject.Controllers;

public static class Utils
{
    public static ProductDTO ProductToDTO(Product product) => new ProductDTO
    {
        ProductId = product.ProductId,
        ProductName = product.ProductName,
        SupplierId = product.SupplierId,
        CategoryId = product.CategoryId,
        UnitPrice = product.UnitPrice,
        QuantityPerUnit = product.QuantityPerUnit
    };

    public static OrderDTO OrderToDTO(Order order)  
    {
        var orderDto = new OrderDTO();
        orderDto.OrderId = order.OrderId;
        orderDto.CustomerId = order.CustomerId;
        orderDto.OrderDate = order.OrderDate;
        orderDto.ShippedDate = order.ShippedDate;
        orderDto.ShipAddress = order.ShipAddress;
        orderDto.ShipCity = order.ShipCity;
        orderDto.ShipRegion = order.ShipRegion;
        orderDto.ShipPostalCode = order.ShipPostalCode;
        orderDto.ShipCountry = order.ShipCountry;
        foreach (var orderDetails in order.OrderDetails)
        {
            orderDto.OrderDetails.Add(OrderDetailToDTO(orderDetails));
        }
        return orderDto;
    }

    public static OrderDetailsDTO OrderDetailToDTO(OrderDetail orderDetail) => new OrderDetailsDTO
    {
        OrderId = orderDetail.OrderId,
        ProductId = orderDetail.ProductId,
        UnitPrice = orderDetail.UnitPrice,
        Quantity = orderDetail.Quantity
    };

    public static CustomerDTO CustomerToDTO(Customer customer) => new CustomerDTO
    {
        CustomerId = customer.CustomerId,
        CompanyName = customer.CompanyName,
        ContactName = customer.ContactName,
        ContactTitle = customer.ContactTitle,
        Address = customer.Address,
        City = customer.City,
        Region = customer.Region,
        Country = customer.Country,
        Phone = customer.Phone,
        Fax = customer.Fax
    };
}

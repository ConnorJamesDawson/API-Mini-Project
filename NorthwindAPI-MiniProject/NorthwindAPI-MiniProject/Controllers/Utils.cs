using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;
using System.Net;

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

    /*
    public static OrderDTO OrderToDTO(Order order) => new OrderDTO
    {
        OrderId = order.OrderId,
        CustomerId = order.CustomerId,
        OrderDate = order.OrderDate,
        ShippedDate = order.ShippedDate,
        ShipAddress = order.ShipAddress,
        ShipCity = order.ShipCity,
        ShipRegion = order.ShipRegion,
        ShipPostalCode = order.ShipPostalCode,
        ShipCountry = order.ShipCountry,
        OrderDetails = order.OrderDetails.Select(od => Utils.OrderToDTO)

    };*/

    public static OrderDTO OrderToDTO(Order order)
    {
        OrderDTO orderDTO = new OrderDTO();
        orderDTO.OrderId = order.OrderId;
        orderDTO.CustomerId = order.CustomerId;
        orderDTO.OrderDate = order.OrderDate;
        orderDTO.ShippedDate = order.ShippedDate;
        orderDTO.ShipAddress = order.ShipAddress;
        orderDTO.ShipCity = order.ShipCity;
        orderDTO.ShipRegion = order.ShipRegion;
        orderDTO.ShipPostalCode = order.ShipPostalCode;
        orderDTO.ShipCountry = order.ShipCountry;

        foreach(var orderDetails in order.OrderDetails)
        {
            orderDTO.OrderDetails.Add(OrderDetailToDTO(orderDetails));
        }

        return orderDTO;
    }

    public static OrderDetailsDTO OrderDetailToDTO(OrderDetail orderDetail) => new OrderDetailsDTO
    {
        OrderId = orderDetail.OrderId,
        ProductId = orderDetail.ProductId,
        UnitPrice = orderDetail.UnitPrice,
        Quantity = orderDetail.Quantity
    };
}

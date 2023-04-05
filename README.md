# API-Mini-Project

## **THIRD-PARTY LIBRARIES**

- Microsoft.AspNetCore.Mvc.NewtonsoftJson v.7.0.4
- Microsoft.AspNetCore.OpenApi v.7.0.4
- Microsoft.EntityFrameworkCore v.7.0.4
- Microsoft.EntityFrameworkCore.SqlServer v.7.0.4
- Microsoft.EntityFrameworkCore.Tools v.7.0.4
- Microsoft.VisualStudio.Web.CodeGeneration.Design v.7.0.4
- Newtonsoft.Json v.13.0.3
- Swashbuckle.AspNetCore v.6.5.0

## **API ACCESS SPECIFICATIONS**

## **CUSTOMER API**

\- GET : https://localhost:7109/api/Customers - Retrieve all customers

\- GET : https://localhost:7109/api/Customers/{id} - Retrieve Single Customer entry for given string id. 

\- GET : https://localhost:7109/api/Customers/{id} /Orders/– Retrieves all of the orders associated with given customer string id. Includes the product details associated with the order.

\- POST : https://localhost:7109/api/Customers - Insert a single customer record. Value Schema: 

{ 

`  `"customerId": "string",

`  `"companyName": "string",

`  `"contactName": "string",

`  `"contactTitle": "string",

`  `"address": "string",

`  `"city": "string",

`  `"region": "string",

`  `"postalCode": "string",

`  `"country": "string",

`  `"phone": "string",

`  `"fax": "string"

}

\- PUT : https://localhost:7109/api/Customers/{id} - Modify customer record for given customer string id. Value Schema:

` `{ 

`  `"customerId": "string",

`  `"companyName": "string",

`  `"contactName": "string",

`  `"contactTitle": "string",

`  `"address": "string",

`  `"city": "string",

`  `"region": "string",

`  `"postalCode": "string",

`  `"country": "string",

`  `"phone": "string",

`  `"fax": "string"

}

\- DELETE : https://localhost:7109/api/Customers/{id} – Removes customer record for given string id.

**ORDERS API**

\- GET : https://localhost:7109/api/Orders - Retrieve all Orders

\- GET : https://localhost:7109/api/Orders/{id} - Retrieve a single order for the given order int id.

\- GET : https://localhost:7109/api/Orders/{customerId} - Retrieve all of the orders that belong to the given customer string id.

\- GET : https://localhost:7109/api/Orders/{id}/OrderDetails - Retrieves all order details associated with the given order int id.

\- GET : https://localhost:7109/api/Orders/{orderId}/OrderDetails/{odId} – Retrieves a single order detail entry associated with an order which matches the passed order int id and order details int id as there element in the List.

\- GET : https://localhost:7109/api/Orders/{orderId}

\- PUT : https://localhost:7109/api/Orders/{id} - Modify an order record. Value Schema:

{

`  `"orderId": 0,

`  `"customerId": "string",

`  `"employeeId": 0,

`  `"orderDate": "2023-04-05T08:09:21.329Z",

`  `"requiredDate": "2023-04-05T08:09:21.329Z",

`  `"shippedDate": "2023-04-05T08:09:21.329Z",

`  `"shipVia": 0,

`  `"freight": 0,

`  `"shipName": "string",

`  `"shipAddress": "string",

`  `"shipCity": "string",

`  `"shipRegion": "string",

`  `"shipPostalCode": "string",

`  `"shipCountry": "string",

`  `"customer": {

`  `}

}

\- PUT : https://localhost:7109/api/Orders/{orderId}/OrderDetails/{ProductId} – Modify an order which matches the passed order id and product id. Value schema:

{

`  `"orderId": 0,

`  `"customerId": "string",

`  `"employeeId": 0,

`  `"orderDate": "2023-04-05T08:09:21.329Z",

`  `"requiredDate": "2023-04-05T08:09:21.329Z",

`  `"shippedDate": "2023-04-05T08:09:21.329Z",

`  `"shipVia": 0,

`  `"freight": 0,

`  `"shipName": "string",

`  `"shipAddress": "string",

`  `"shipCity": "string",

`  `"shipRegion": "string",

`  `"shipPostalCode": "string",

`  `"shipCountry": "string",

`  `"customer": {

`  `}

}

\- POST : https://localhost:7109/api/Orders/ - Creates an order entry. Value Schema:

` `{

`  `"orderId": 0,

`  `"customerId": "string",

`  `"employeeId": 0,

`  `"orderDate": "2023-04-05T08:09:21.329Z",

`  `"requiredDate": "2023-04-05T08:09:21.329Z",

`  `"shippedDate": "2023-04-05T08:09:21.329Z",

`  `"shipVia": 0,

`  `"freight": 0,

`  `"shipName": "string",

`  `"shipAddress": "string",

`  `"shipCity": "string",

`  `"shipRegion": "string",

`  `"shipPostalCode": "string",

`  `"shipCountry": "string",

`  `"customer": {

`  `}

}

\- POST : https://localhost:7109/api/Orders/{orderId}/OrderDetails – Creates an order details entry associated to the given order id. Value Schema:

{

`  `"orderId": 0,

`  `"productId": 0,

`  `"unitPrice": 0,

`  `"quantity": 0,

`  `"discount": 0

}

\- DELETE : https://localhost:7109/api/Orders/{orderId} – Deletes order record for given order int id.

\- DELETE : https://localhost:7109/api/Orders/{orderId}/OrderDetails/orderDetailsId – Deletes an order details associated with the order of the passed order int id and matching the order details int id.

**PRODUCTS API**

\- GET : https://localhost:7109/api/Products - Retrieves all products

\- GET : https://localhost:7109/api/Products/{id} - Retrieves a single product by given int id.

\- POST : https://localhost:7109/api/Products - Insert a single Product record. Value Schema:

{

`  `"productId": 0,

`  `"productName": "string",

`  `"supplierId": 0,

`  `"categoryId": 0,

`  `"quantityPerUnit": "string",

`  `"unitPrice": 0,

`  `"unitsInStock": 0,

`  `"unitsOnOrder": 0,

`  `"reorderLevel": 0,

`  `"discontinued": true

}

\- PUT : https://localhost:7109/api/Products - Modify a single product record. Value Schema:

{

`  `"productId": 0,

`  `"productName": "string",

`  `"supplierId": 0,

`  `"categoryId": 0,

`  `"quantityPerUnit": "string",

`  `"unitPrice": 0,

`  `"unitsInStock": 0,

`  `"unitsOnOrder": 0,

`  `"reorderLevel": 0,

`  `"discontinued": true

}

\- DELETE : https://localhost:7109/api/Products - Delete a single product record for given int id.

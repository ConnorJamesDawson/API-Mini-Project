using Microsoft.AspNetCore.Mvc;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;
using static System.Net.WebRequestMethods;

namespace NorthwindAPI_MiniProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly INorthwindService<Product> _service;

        public ProductsController(
            INorthwindService<Product> service)
        {
            _service = service;

        }

        // GET: api/Products
        [HttpGet(Name = nameof(GetProducts))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _service.GetAllAsync();
            if (products == null)
            {
                return NotFound();
            }
            var productDtos = products.Select(p => CreateProductLinks(Utils.ProductToDTO(p))).ToList();


            return productDtos;
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = nameof(GetProduct))]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _service.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductDTO productDto = Utils.ProductToDTO(product);

            return CreateProductLinks(productDto);

        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutProduct))]
        public async Task<IActionResult> PutProduct(int id,
            [Bind("ProductId, ProductName, UnitPrice, UnitsInStock, SupplierId")] Product product)
        {

            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var updatedSuccessfully = await _service.UpdateAsync(id, product);

            if (!updatedSuccessfully)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = nameof(PostProduct))]
        public async Task<ActionResult<ProductDTO>> PostProduct(
            [Bind("ProductName, UnitPrice, UnitsInStock, SupplierId")] Product product)
        {

            var createdSuccessfully = await _service.CreateAsync(product);
            if (!createdSuccessfully)
            {
                return Problem($"Entity set 'NorthwindContext.Products'  is null or entity with id: {product.ProductId} already exists");
            }
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, CreateProductLinks(Utils.ProductToDTO(product)));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}", Name = nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            var deletedSuccessfully = await _service.DeleteAsync(id);
            if (!deletedSuccessfully)
            {
                return NotFound();
            }
            return NoContent();
        }


        private ProductDTO CreateProductLinks(ProductDTO product)
        {
            if (Url == null) return product;
            var idObj = new { id = product.ProductId };
            product.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetProduct), idObj),
                "self",
                "GET"));

            product.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PostProduct), idObj),
                "post_product",
                "POST"));

            product.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutProduct), idObj),
                "delete_product",
                "DELETE"));

            return product;
        }
    }
}


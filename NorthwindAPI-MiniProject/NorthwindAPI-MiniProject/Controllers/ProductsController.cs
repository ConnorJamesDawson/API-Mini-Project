using Microsoft.AspNetCore.Mvc;
using NorthwindAPI_MiniProject.Models;
using NorthwindAPI_MiniProject.Models.DTO;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _service.GetAllAsync();
            if (products == null)
            {
                return NotFound();
            }

            return products.Select(Utils.ProductToDTO).ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _service.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Utils.ProductToDTO(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(
            [Bind("ProductId, ProductName, UnitPrice, UnitsInStock, SupplierId")] Product product)
        {

            var createdSuccessfully = await _service.CreateAsync(product);
            if (!createdSuccessfully)
            {
                return Problem($"Entity set 'NorthwindContext.Products'  is null or entity with id: {product.ProductId} already exists");
            }
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, Utils.ProductToDTO(product));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            var deletedSuccessfully = await _service.DeleteAsync(id);
            if (!deletedSuccessfully)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

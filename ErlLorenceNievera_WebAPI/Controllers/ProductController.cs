using ErlLorenceNievera_WebAPI.Data;
using ErlLorenceNievera_WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErlLorenceNievera_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Product/search/phone
        [HttpGet("search/{name}")]
        public async Task<ActionResult<Product>> SearchProduct(string name)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name.Contains(name));

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Product/SearchPartialProductName/ap
        [HttpGet("SearchPartialProductName/{name}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchPartialProductName(string name)
        {
            var products = await _context.Products
                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                .ToListAsync();

            if (products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully!");
        }
    }
}

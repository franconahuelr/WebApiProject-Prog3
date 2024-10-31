using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;


namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbApiProjectContext _dbApiProjectContext;
        public ProductController(DbApiProjectContext dbApiProjectContext)
        {
            _dbApiProjectContext = dbApiProjectContext;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("List")]
        public async Task<IActionResult> List(string? searchTerm = null)
        {
            var productsQuery = _dbApiProjectContext.Products.AsQueryable();

            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm)); // Filtra por nombre
            }

            var list = await productsQuery.ToListAsync();

            return Ok(new
            {
                isSuccess = true,
                products = list
            });
        }
        [HttpPost]
       [Authorize(Roles = "admin")]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("El producto no puede ser nulo.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dbApiProjectContext.Products.AddAsync(product);
            await _dbApiProjectContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { message = "Producto agregado exitosamente.", product });
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbApiProjectContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Producto no encontrado." }); // 404 Not Found
            }

            _dbApiProjectContext.Products.Remove(product);
            await _dbApiProjectContext.SaveChangesAsync();

            return Ok(new { message = "Producto eliminado exitosamente." }); // 200 OK
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("El producto no puede ser nulo.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _dbApiProjectContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Producto no encontrado." }); // 404 Not Found
            }

            product.Name = updatedProduct.Name;
            product.Brand = updatedProduct.Brand;
            product.Price = updatedProduct.Price;
           
            await _dbApiProjectContext.SaveChangesAsync();

            return Ok(new { message = "Producto actualizado exitosamente.", product });
        }
    }
}
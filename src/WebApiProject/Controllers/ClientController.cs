using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly DbApiProjectContext _context;
        public ClientController(IClientService clientService, DbApiProjectContext context)
        {
            _clientService = clientService;
            _context = context;
        }

        [HttpPost("/cart/items/add")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> AddToCart(string userName, [FromBody] CartItem item)
        {
            if (item == null || item.Quantity <= 0)
            {
                return StatusCode(400, "Invalid item data."); // 400 Bad Request
            }

            await _clientService.AddToCart(userName, item);
            return StatusCode(201, "Producto agregado exitosamente"); // 201 Created
        }
        
 
        [HttpDelete("/cart/items/delete")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> RemoveFromCart(string userName, [FromBody] CartItem item)
        {
            if (item == null || item.ProductId <= 0) 
            {
                return StatusCode(400, "Invalid item data."); // 400 Bad Request
            }

            await _clientService.RemoveFromCart(userName, item);
            return StatusCode(204, new {messege ="Producto removido con exito"}); // 204 No Content
        }

        [HttpGet("/cart/items")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> GetCartItems(string userName)
        {
            var items = await _clientService.GetCartItems(userName);

            return StatusCode(200, items); // 200 OK
        }
        [HttpPost("/cart")]
        [Authorize(Roles = "client")]
        public async Task CreateCartForClient(string username)
        {
            var client = await _context.Users.FirstOrDefaultAsync(c => c.UserName == username);
            if (client == null) throw new ArgumentException("Client no encontrado.");

            // Crear un nuevo carrito
            var newCart = new ShoppingCart { UserId = username, CartItems = new List<CartItem>() };
            _context.ShoppingCarts.Add(newCart);
            await _context.SaveChangesAsync();
           
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models.DTOs;


namespace WebApiProject.Controllers {

    [Route("api/[controller]")]
    [ApiController]
   
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<ShoppingCartDto>> GetCart(string userId)
        {
            var cart = await _shoppingCartService.GetCartAsync(userId);
            if (cart == null)
            {
                return NotFound("Carrito no encontrado.");
            }

            return Ok(new ShoppingCartDto
            {
                IdCart = cart.IdCart,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost("{userId}/items")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<CartItemDto>> AddItem(string userId, [FromBody] CartItemDto itemDto)
        {
            try
            {
                var cartItem = await _shoppingCartService.AddItemToCartAsync(userId, itemDto);
                return CreatedAtAction(nameof(GetCart), new { userId }, cartItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> UpdateCart(string userId, [FromBody] ShoppingCartDto cartDto)
        {
            await _shoppingCartService.UpdateCartAsync(userId, cartDto);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> DeleteCart(string userId)
        {
            await _shoppingCartService.DeleteCartAsync(userId);
            return NoContent();
        }
    }
}
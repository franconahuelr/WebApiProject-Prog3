using WebApiProject.Models.DTOs;
using WebApiProject.Models.Entities;

namespace WebApiProject.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartAsync(string userId);
        Task<ShoppingCart> CreateCartAsync(ShoppingCartDto cartDto);
        Task UpdateCartAsync(string userId, ShoppingCartDto cartDto);
        Task DeleteCartAsync(string userId);
        Task<CartItem> AddItemToCartAsync(string userId, CartItemDto itemDto);
    }
}
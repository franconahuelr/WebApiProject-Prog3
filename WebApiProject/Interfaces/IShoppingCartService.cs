using WebApiProject.Models.DTOs;
using WebApiProject.Models.Entities;

namespace WebApiProject.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartAsync(string userId);
        Task<ShoppingCart> CreateCartAsync(string userName);
        Task<CartItem> AddItemToCartAsync(string userId, CartItemDto itemDto);
        Task UpdateCartItemAsync(string userId, CartItemDto itemDto);
        Task RemoveItemFromCartAsync(string userId, int productId); // Nuevo método
        Task DeleteCartAsync(string userId);
        Task UpdateCartAsync(string userId, ShoppingCartDto cartDto);
    }
}

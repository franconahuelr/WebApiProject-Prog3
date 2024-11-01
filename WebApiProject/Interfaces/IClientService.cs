using WebApiProject.Models.Entities;

public interface IClientService
{
    Task AddToCart(string userId, CartItem item);
    Task RemoveFromCart(string userId, CartItem item);
    Task<List<CartItem>> GetCartItems(string userId);
    Task CreateCartForClient(string userId);
}
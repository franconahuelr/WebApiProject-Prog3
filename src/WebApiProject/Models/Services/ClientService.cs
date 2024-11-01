
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;
using Microsoft.EntityFrameworkCore;


public class ClientService : IClientService
{
    private readonly DbApiProjectContext _context;
    private readonly ILogger<ClientService> _logger;

    public ClientService(DbApiProjectContext context, ILogger<ClientService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddToCart(string userId, CartItem item)
    {
        var client = await _context.Clients.Include(c => c.CartItems)
                                            .FirstOrDefaultAsync(c => c.IdUser == userId);
        if (client == null) throw new ArgumentException("No se encontró el cliente.");

        // Verificar si el item ya existe en el carrito
        var existingItem = client.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            client.CartItems.Add(item);
        }

        await _context.SaveChangesAsync(); 
    }

    public async Task RemoveFromCart(string userId, CartItem item)
    {
        var client = await _context.Clients.Include(c => c.CartItems)
                                            .FirstOrDefaultAsync(c => c.IdUser == userId);
        if (client == null) throw new ArgumentException("No se encontró el cliente.");

        var existingItem = client.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
        if (existingItem != null)
        {
            client.CartItems.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetCartItems(string userId)
    {
        var client = await _context.Clients.Include(c => c.CartItems)
                                            .FirstOrDefaultAsync(c => c.IdUser == userId);
        if (client == null) throw new ArgumentException("No se encontró el cliente.");

        return client.CartItems.ToList();
    }
    public async Task CreateCartForClient(string userId)
    {
        try
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.IdUser == userId);
            if (client == null)
            {
                _logger.LogWarning($"El cliente con el ID {userId} no fue encontrado.");
                throw new ArgumentException("No se encontro al cliente.");
            }

            var newCart = new ShoppingCart { UserId = userId, CartItems = new List<CartItem>() };
            _context.ShoppingCarts.Add(newCart);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el Carrito para el cliente");
            throw; // Propagar la excepción para que sea manejada por el controlador
        }
    }
}
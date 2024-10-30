using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Interfaces;
using WebApiProject.Models.Context;
using WebApiProject.Models.DTOs;
using WebApiProject.Models.Entities;
namespace WebApiProject.Models
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DbApiProjectContext _dbApiProjectContext;

        public ShoppingCartService(DbApiProjectContext dbApiProjectContext)
        {
            _dbApiProjectContext = dbApiProjectContext;
        }

        // Obtiene el carrito de compras de un usuario
        public async Task<ShoppingCart> GetCartAsync(string userId)
        {
            return await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product) // Incluye la información del producto
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Crea un nuevo carrito de compras o agrega un producto al carrito existente
        public async Task<CartItem> AddItemToCartAsync(string userId, CartItemDto itemDto)
        {
            var product = await _dbApiProjectContext.Products.FindAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException("El producto no existe.");
            }

            var cart = await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _dbApiProjectContext.ShoppingCarts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += itemDto.Quantity; // Actualiza la cantidad si ya existe
            }
            else
            {
                cart.Items.Add(item: new CartItem { ProductId = itemDto.ProductId, Quantity = itemDto.Quantity, Product=product });
            }

            await _dbApiProjectContext.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
        }

        // Actualiza un item en el carrito
        public async Task UpdateCartItemAsync(string userId, CartItemDto itemDto)
        {
            var cart = await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new ArgumentException("Carrito no encontrado.");
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity = itemDto.Quantity; // Actualiza la cantidad
            }
            else
            {
                throw new ArgumentException("El producto no está en el carrito.");
            }

            await _dbApiProjectContext.SaveChangesAsync();
        }

        //Elimina un item del carrito
      public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new ArgumentException("Carrito no encontrado.");
            }
    
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem); // Elimina el item del carrito
                await _dbApiProjectContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("El producto no está en el carrito.");
            }
        }

        // Elimina todo el carrito de un usuario
        public async Task DeleteCartAsync(string userId)
        {
            var cart = await _dbApiProjectContext.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _dbApiProjectContext.ShoppingCarts.Remove(cart);
                await _dbApiProjectContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Carrito no encontrado.");
            }
        }

        public Task<ShoppingCart> CreateCartAsync(ShoppingCartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCartAsync(string userId, ShoppingCartDto cartDto)
        {
            throw new NotImplementedException();
        }
    }
}
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

        // Crea un nuevo carrito de compras
        public async Task<ShoppingCart> CreateCartAsync(string userId)
        {
            var cart = new ShoppingCart { UserId = userId };
            await _dbApiProjectContext.ShoppingCarts.AddAsync(cart);
            await _dbApiProjectContext.SaveChangesAsync();
            return cart;
        }

        // Obtiene el carrito de compras de un usuario
        public async Task<ShoppingCart> GetCartAsync(string userId)
        {
            return await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Agrega un artículo al carrito
        public async Task<CartItem> AddItemToCartAsync(string userId, CartItemDto itemDto)
        {
            var product = await _dbApiProjectContext.Products.FindAsync(itemDto.ProductId);
            if (product == null)
                throw new ArgumentException("El producto no existe.");

            var cart = await GetCartAsync(userId) ?? await CreateCartAsync(userId);

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += itemDto.Quantity; // Actualiza la cantidad si ya existe
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Product = product
                };
                cart.CartItems.Add(cartItem);
            }

            await _dbApiProjectContext.SaveChangesAsync();
            return cartItem;
        }

        // Quita un artículo del carrito
        public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await GetCartAsync(userId);
            if (cart == null)
                throw new ArgumentException("Carrito no encontrado.");

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem); // Elimina el item del carrito
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
            var cart = await GetCartAsync(userId);
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

        // Actualiza un item en el carrito (opcional, puedes implementarlo si es necesario)
        public async Task UpdateCartItemAsync(string userId, CartItemDto itemDto)
        {
            var cart = await GetCartAsync(userId);
            if (cart == null)
                throw new ArgumentException("Carrito no encontrado.");

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity = itemDto.Quantity; // Actualiza la cantidad
                await _dbApiProjectContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("El producto no está en el carrito.");
            }
        }
        public async Task UpdateCartAsync(string userId, ShoppingCartDto cartDto)
        {
            // Obtiene el carrito existente
            var cart = await _dbApiProjectContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new ArgumentException("Carrito no encontrado.");
            }

            // Actualiza los items del carrito
            cart.CartItems.Clear(); // Opcional: Limpia los items actuales

            foreach (var itemDto in cartDto.CartItems)
            {
                var product = await _dbApiProjectContext.Products.FindAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"El producto con ID {itemDto.ProductId} no existe.");
                }

                cart.CartItems.Add(new CartItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Product = product // Asigna el producto si es necesario
                });
            }

            await _dbApiProjectContext.SaveChangesAsync();
        }
    }
}

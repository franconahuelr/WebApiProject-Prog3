namespace WebApiProject.Models.Entities
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public int IdCart { get; set; }
        public string UserId { get; set; } // Referencia al usuario
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
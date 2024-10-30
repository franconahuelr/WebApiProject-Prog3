namespace WebApiProject.Models.Entities
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Referencia al usuario
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
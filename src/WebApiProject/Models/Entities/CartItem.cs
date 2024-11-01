namespace WebApiProject.Models.Entities
{
    public class CartItem
    {
        public int IdCartItem { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual  Product Product { get; set; }
        
        public virtual  ShoppingCart ShoppingCart { get; set; }
    }
}
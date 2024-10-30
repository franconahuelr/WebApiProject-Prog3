namespace WebApiProject.Models.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual required Product Product { get; set; }
        
    }
}
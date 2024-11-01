

namespace WebApiProject.Models.Entities
{
    public class Client : User
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }

}
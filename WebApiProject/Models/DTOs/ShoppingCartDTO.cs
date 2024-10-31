namespace WebApiProject.Models.DTOs;
using System.Collections.Generic;

public class ShoppingCartDto
{
    public int IdCart { get; set; }
    public string UserId { get; set; }
    public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}
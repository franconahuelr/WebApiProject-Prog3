namespace WebApiProject.Models.DTOs;
using System.Collections.Generic;

public class ShoppingCartDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}
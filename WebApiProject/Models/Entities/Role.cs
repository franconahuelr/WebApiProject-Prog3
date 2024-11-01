using System.Security.Claims;
using WebApiProject.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<User> Users { get; set; } // Navegación para los usuarios asociados
    
}
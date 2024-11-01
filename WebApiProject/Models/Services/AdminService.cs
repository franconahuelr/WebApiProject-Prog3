using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

public class AdminService : IAdminService
{
    private readonly DbApiProjectContext _context;

    public AdminService(DbApiProjectContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync(); 
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("El usuario no fue encontrado");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
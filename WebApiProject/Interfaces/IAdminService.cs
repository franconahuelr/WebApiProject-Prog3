using WebApiProject.Models.Entities;

public interface IAdminService
{
    Task<List<User>> GetAllUsersAsync();
    Task DeleteUserAsync(string userId);
}
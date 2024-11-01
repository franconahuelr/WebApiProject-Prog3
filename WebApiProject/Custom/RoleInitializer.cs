using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

namespace WebApiProject.Custom
{
    public class RoleInitializer
    {
        public static async Task Initialize(DbApiProjectContext context, Utilities utilities)
        {
            await context.Database.EnsureCreatedAsync();

            // Crear roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "admin" },
                    new Role { Name = "client" }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // Crear usuario administrador
            var adminEmail = "admin@example.com";
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (adminUser == null)
            {
                string adminPassword = "Admin@123";
                string encryptedPassword = utilities.EncryptSHA256(adminPassword);

                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Password = encryptedPassword,
                    RoleId = (await context.Roles.FirstAsync(r => r.Name == "admin")).Id // Asignar rol de admin
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }

            // Crear usuario cliente
            var clientEmail = "client@example.com";
            var clientUser = await context.Users.FirstOrDefaultAsync(u => u.Email == clientEmail);

            if (clientUser == null)
            {
                string clientPassword = "Client@123";
                string encryptedPassword = utilities.EncryptSHA256(clientPassword);

                clientUser = new User
                {
                    UserName = "client",
                    Email = clientEmail,
                    Password = encryptedPassword,
                    RoleId = (await context.Roles.FirstAsync(r => r.Name == "client")).Id // Asignar rol de client
                };

                await context.Users.AddAsync(clientUser);
                await context.SaveChangesAsync();
            }

        }
    }
}
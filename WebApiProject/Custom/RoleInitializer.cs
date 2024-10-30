﻿using Microsoft.AspNetCore.Identity;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

namespace WebApiProject.Custom
{


    public class RoleInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, Utilities utilities)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = serviceProvider.GetRequiredService<DbApiProjectContext>();

            // Crear roles
            string[] roleNames = { "admin", "user" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Crear usuario administrador
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = "admin", Email = adminEmail };
                string adminPassword = "Admin@123";

                // Crea el usuario con el UserManager (se encriptará automáticamente)
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");

                    // Guardar también en la tabla UserDatas con la contraseña encriptada
                    var userData = new UserData
                    {
                        UserName = adminUser.UserName,
                        Email = adminUser.Email,
                        Password = utilities.encriptSHA256(adminPassword), 
                        IsAdmin = true
                    };

                    await dbContext.UserDatas.AddAsync(userData);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error al crear usuario: {error.Description}");
                    }
                }
            }
        }
    }
}
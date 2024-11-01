using System;
using System.Collections.Generic;

namespace WebApiProject.Models.Entities;

public partial class User
{
    public string IdUser { get; set; } = Guid.NewGuid().ToString();

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; } // ID del rol asociado

    public Role Role { get; set; } // Navegación
}

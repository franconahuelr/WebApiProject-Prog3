using System;
using System.Collections.Generic;

namespace WebApiProject.Models.Entities;

public partial class User
{
    public string IdUser { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }
}

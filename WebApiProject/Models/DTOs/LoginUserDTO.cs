using System.ComponentModel.DataAnnotations;

namespace WebApiProject.Models.DTOs
{
    public class LoginUserDTO
    {
            [Required(ErrorMessage = "El correo electrónico es requerido.")]
            [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida.")]
            public string Password { get; set; }
        }
    }
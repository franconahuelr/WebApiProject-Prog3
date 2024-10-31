using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WebApiProject.Custom;
using WebApiProject.Models.Entities;
using WebApiProject.Models.Context;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
    
        private readonly Utilities _utilities;
        private readonly DbApiProjectContext _dbApiProjectContext;

        public AccessController(UserManager<IdentityUser> userManager, Utilities utilities, DbApiProjectContext dbApiProjectContext)
        {
            _userManager = userManager;
   
            _utilities = utilities;
            _dbApiProjectContext = dbApiProjectContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userModel = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            // Intentar crear el usuario
            var result = await _userManager.CreateAsync(userModel, user.Password);

            if (result.Succeeded)
            {
                // Obtener el ID generado automáticamente
                var userId = userModel.Id;

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Usuario registrado con éxito",
                    isSuccess = true,
                    userId // Devuelve el ID generado
                });
            }
            else
            {
                return BadRequest(new { isSuccess = false, errors = result.Errors });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserDTO user)
        {
            // Busca al usuario por email
            var userFound = await _userManager.FindByEmailAsync(user.Email);
            if (userFound == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario Incorrecto", isSuccess = false, token = "" });
            }

            // Verifica la contraseña
            var result = await _userManager.CheckPasswordAsync(userFound, user.Password);
            if (!result)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario Incorrecto", isSuccess = false, token = "" });
            }

            // Generar el token si el usuario es encontrado
            var token = _utilities.GenerateJWT(userFound);
            return StatusCode(StatusCodes.Status200OK, new { message = "Ingreso con éxito", isSuccess = true, token });
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApiProject.Custom;
using WebApiProject.Models.Entities;
using WebApiProject.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApiProject.Controllers
{
    //Por falta de tiempo no llegue a modificar accesscontroller y productcontroller para que la logica la manejen sus servicios, pero admin y client si tienen sus interfaces y servicios

    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly Utilities _utilities;
        private readonly DbApiProjectContext _dbApiProjectContext;

        public AccessController(Utilities utilities, DbApiProjectContext dbApiProjectContext)
        {
            _utilities = utilities;
            _dbApiProjectContext = dbApiProjectContext;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica si el usuario ya existe
            var existingUser = await _dbApiProjectContext.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El usuario ya existe." });
            }

            // Crear el nuevo usuario
            var userModel = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = _utilities.EncryptSHA256(user.Password),
                RoleId = (int)((await _dbApiProjectContext.Roles.FirstOrDefaultAsync(r => r.Name == "client"))?.Id) // Asignar RoleId para "client"
            };

            if (userModel.RoleId == null)
            {
                return BadRequest(new { message = "El rol 'client' no existe." });
            }

            await _dbApiProjectContext.Users.AddAsync(userModel);
            await _dbApiProjectContext.SaveChangesAsync();


            return StatusCode(StatusCodes.Status200OK, new
            {
                message = "Usuario registrado con éxito",
                isSuccess = true,
                userId = userModel.IdUser
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO user)
        {
            // Busca al usuario por email
            var userFound = await _dbApiProjectContext.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userFound == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario Incorrecto", isSuccess = false, token = "" });
            }

            // Verifica la contraseña
            var hashedPassword = _utilities.EncryptSHA256(user.Password);
            if (userFound.Password != hashedPassword)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario Incorrecto", isSuccess = false, token = "" });
            }

            // Generar el token si el usuario es encontrado
            var token = _utilities.GenerateJWT(userFound);
            return StatusCode(StatusCodes.Status200OK, new { message = "Ingreso con éxito", isSuccess = true, token });
        }
    }
}
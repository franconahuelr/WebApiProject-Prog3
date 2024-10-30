using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Custom;
using WebApiProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApiProject.Models.Context;
using WebApiProject.Models.Entities;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly DbApiProjectContext _dbApiProjectContext;
        private readonly Utilities _utilities;
        public AccessController(DbApiProjectContext dbApiProjectContext, Utilities utilities)
        {
            _dbApiProjectContext = dbApiProjectContext;
            _utilities = utilities;
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

            var userModel = new UserData
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = _utilities.encriptSHA256(user.Password)
            };

            await _dbApiProjectContext.UserDatas.AddAsync(userModel);
            await _dbApiProjectContext.SaveChangesAsync();

            if (userModel.IdUser != 0)
                return StatusCode(StatusCodes.Status200OK, new {messege= "Usuario Registrado con exito", isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserDTO user)
        {
            // Encripta la contraseña de entrada
            var hashedPassword = _utilities.encriptSHA256(user.Password);

            // Busca en la tabla UserDatas
            var userFound = await _dbApiProjectContext.UserDatas
                .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == hashedPassword);

            if (userFound == null)
                return StatusCode(StatusCodes.Status200OK, new { messege = "Usuario Incorrecto", isSuccess = false, token = "" });

            // Generar el token si el usuario es encontrado
            return StatusCode(StatusCodes.Status200OK, new { messege = "Ingreso con éxito", isSuccess = true, token = _utilities.generateJWT(userFound) });
        }
    }
}

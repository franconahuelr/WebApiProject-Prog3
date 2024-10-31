using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")] 
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Método para obtener todos los usuarios
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userDtos = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
               
            }).ToList();

            return Ok(new { isSuccess = true, users = userDtos });
        }

        // Método para eliminar un usuario por su ID
        [HttpDelete]
        [Route("Users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { isSuccess = false, message = "Usuario no encontrado." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { isSuccess = true, message = "Usuario eliminado con éxito." });
            }

            return BadRequest(new { isSuccess = false, errors = result.Errors });
        }
    }
}
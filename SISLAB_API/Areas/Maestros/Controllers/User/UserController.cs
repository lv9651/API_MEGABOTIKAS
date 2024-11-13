using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SISLAB_API.Areas.Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        // Inyección de dependencias del servicio UserService
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Obtener todos los roles
        [HttpGet("role")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRolAsync()
        {
            var roles = await _userService.GetAllRolsAsync();
            return Ok(roles);
        }

        // Actualizar usuario
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] User user)
        {
            // Asegurarse de que el id del usuario en la URL coincida con el id del cuerpo
            if (id != user.id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del usuario.");
            }

            // Intentar actualizar el usuario
            var updateResult = await _userService.UpdateUserAsync(user);

            if (updateResult)
            {
                return NoContent(); // 204 No Content -> Actualización exitosa
            }
            else
            {
                return NotFound("El usuario no fue encontrado o no se pudo actualizar.");
            }
        }
    }
}
using API_MEGABOTIKAS.Areas.Maestros.Models.Usuario;
using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;

namespace SISLAB_API.Areas.Maestros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioServicio _servicio;

        public UsuarioController(UsuarioServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpPost("validarUsuario_Login")]
        public async Task<IActionResult> validarUsuario_Login([FromBody] DTOLogin login)
        {
            if (string.IsNullOrWhiteSpace(login.Usuario) || string.IsNullOrWhiteSpace(login.Dni))
            {
                return BadRequest("Usuario o DNI requeridos.");
            }

            var esValido = await _servicio.validarUsuario_Login(login.Usuario, login.Dni);

            if (!esValido)
                return Unauthorized("Usuario o credenciales inválidas.");

            return Ok(new { message = "Login correcto" });
        }
    }
}
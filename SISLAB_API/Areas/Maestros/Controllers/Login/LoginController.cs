using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;
using System.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SISLAB_API.Areas.Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest(new { message = "Nombre de usuario y contraseña son requeridos" });
            }

            try
            {
                var user = await _loginService.AuthenticateUserAsync(request.username, request.password);

                if (user == null)
                {
                    return Unauthorized(new { message = "Nombre de usuario o contraseña incorrectos" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Inicio de sesión exitoso",
                    user = new
                    {
                       id=user.id,
                       username = user.username,
                       dni= user.dni,
                       role= user.role_id,
                       nombre = user.nombre

                    }
                });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor" });
            }
        }





        [HttpPost("reg-user")]
        public async Task<IActionResult> AddUsuario( [FromBody] Usuario usuario)
        {
            // Validar que el objeto 'usuario' no sea nulo y que tenga datos válidos
            if (usuario == null || string.IsNullOrEmpty(usuario.Username) || string.IsNullOrEmpty(usuario.clave))
            {
                return BadRequest(new { message = "Datos de usuario inválidos." });
            }

            try
            {
                // Llamar al servicio para agregar el usuario
                await _loginService.AddUserAsync(usuario);
                return StatusCode(201, new { message = "Usuario agregado exitosamente." });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { message = $"Error al agregar el usuario: {ex.Message}" });
            }
        }

    }












}


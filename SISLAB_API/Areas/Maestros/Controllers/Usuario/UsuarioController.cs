using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;
using System.Web;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity.Data;




namespace SISLAB_API.Areas.Maestros.Controllers // Cambia esto al espacio de nombres real
{

    [Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
        private readonly UsuarioServicio _servicio;

        public UsuarioController(UsuarioServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioVinali usuario)
        {
            try
            {
                await _servicio.RegistrarUsuario(usuario);
                return Ok(new { mensaje = "Usuario registrado con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            Console.WriteLine($"📥 Login recibido: Correo={request.Correo}, Contrasena={request.Contrasena}");

            var usuario = await _servicio.ObtenerPorCorreo(request.Correo);
            if (usuario == null)
                return Unauthorized(new { error = "Usuario no encontrado" });

            if (!_servicio.VerificarContrasena(request.Contrasena, usuario.ContrasenaHash))
                return Unauthorized(new { error = "Contraseña incorrecta" });

            return Ok(new
            {
                mensaje = "Login exitoso",
                usuario = new
                {
                    usuario.IdUsuario,
                    usuario.Nombre
                }
            });
        }


        [HttpPost("recuperar")]
        public async Task<IActionResult> Recuperar([FromBody] RecuperarRequest data)
        {
            var usuario = await _servicio.ObtenerPorCorreo(data.Correo);
            if (usuario == null) return NotFound(new { error = "Correo no registrado" });

            string codigo = new Random().Next(100000, 999999).ToString();
            await _servicio.GuardarCodigoRecuperacion(data.Correo, codigo);

            // ✅ Enviar el código al correo del usuario
            await _servicio.EnviarCorreoRecuperacion(data.Correo, codigo);

            return Ok(new { mensaje = "Código de recuperación enviado" });
        }

        [HttpPost("resetear-contrasena")]
        public async Task<IActionResult> Resetear([FromBody] ResetearRequest data)
        {
            var usuario = await _servicio.ObtenerPorCorreo(data.Correo);
            if (usuario == null) return NotFound(new { error = "Usuario no encontrado" });

            if (usuario.CodigoRecuperacion != data.Codigo || usuario.FechaExpiracionCodigo < DateTime.Now)
                return BadRequest(new { error = "Código inválido o expirado" });

            await _servicio.ActualizarContrasena(data.Correo, data.NuevaContrasena);
            return Ok(new { mensaje = "Contraseña actualizada correctamente" });
        }
    }
}
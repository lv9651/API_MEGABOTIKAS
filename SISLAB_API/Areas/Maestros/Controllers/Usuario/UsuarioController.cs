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
        private readonly EmailService _emailService;

        public UsuarioController(UsuarioServicio servicio, EmailService emailService)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost("validar-compra")]
        public async Task<IActionResult> ValidarCompra([FromBody] PurchaseValidationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NumeroDocumento))
                return BadRequest(new { message = "NumeroDocumento es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.TipoDocumento))
                return BadRequest(new { message = "TipoDocumento es obligatorio." });

            var result = await _servicio.ValidateLastPurchaseAsync(dto);
            if (result.IsValid)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpGet("validar-correo")]
        public async Task<IActionResult> ValidarCorreo([FromQuery] string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return BadRequest(new { mensaje = "Correo inválido" });

            bool existe = await _servicio.ValidarCorreoAsync(correo);

            if (!existe)
                return BadRequest(new { mensaje = "El correo no está registrado" });

            return Ok(new { mensaje = "Correo válido", existe });
        }
        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] UsuarioDto dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "Datos inválidos" });

            // Convertir DTO a modelo
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                ApellidoPaterno = dto.ApellidoPaterno ?? "",
                ApellidoMaterno = dto.ApellidoMaterno ?? "",
                Correo = dto.Correo,
                TipoDocumento = dto.TipoDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                FechaRegistro = DateTime.Now
            };

            // Registrar o validar usuario
            var result = await _servicio.SocialLoginAsync(usuario);

            // Si hay mensaje de error, devolverlo sin enviar correo
            if (result == null || !string.IsNullOrEmpty(result.Mensaje))
                return BadRequest(new { mensaje = result?.Mensaje ?? "No se pudo realizar el login social." });

            // ✅ Solo si todo salió bien, enviamos correo
            try
            {
                await _emailService.SendConfirmationEmailAsync(usuario.Correo, usuario.Nombre);
            }
            catch
            {
                // Loggear el error, pero no interrumpir la respuesta
                Console.WriteLine("No se pudo enviar el correo de confirmación.");
            }

            return Ok(result);
        }
    }
}
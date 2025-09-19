using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services; // <-- Aquí tu servicio de usuarios

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly EmailService _emailService;

    public UsuarioController(UsuarioServicio usuarioServicio, EmailService emailService)
    {
        _usuarioServicio = usuarioServicio;
        _emailService = emailService;
    }

    [HttpPost("social-loginn")]
    public async Task<IActionResult> SocialLogin([FromBody] Usuario usuario)
    {
        // Registramos o validamos el usuario usando el servicio
        var result = await _usuarioServicio.SocialLoginAsync(usuario);

        if (!string.IsNullOrEmpty(result.Mensaje))
            return BadRequest(new { mensaje = result.Mensaje });

        // Enviar correo de confirmación
        await _emailService.SendConfirmationEmailAsync(usuario.Correo, usuario.Nombre);

        return Ok(result);
    }
}
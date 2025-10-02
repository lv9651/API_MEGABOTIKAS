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

    [HttpGet("TESTTTTT")]
    public string Test()
    {
        return "TEst;";
    }
}
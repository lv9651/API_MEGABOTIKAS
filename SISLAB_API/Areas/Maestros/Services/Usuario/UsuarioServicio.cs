using Org.BouncyCastle.Crypto.Generators;
using SISLAB_API.Areas.Maestros.Models;

using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class UsuarioServicio
    {
        private readonly UsuarioRepositorio _UsuarioRepository;

        public UsuarioServicio(UsuarioRepositorio UsuarioRepository)
        {
            _UsuarioRepository = UsuarioRepository;
        }



        public async Task RegistrarUsuario(UsuarioVinali usuario)
        {
            // Aquí deberías encriptar la contraseña antes de guardarla
            usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.ContrasenaHash);
            await _UsuarioRepository.RegistrarUsuario(usuario);
        }

        public async Task<UsuarioVinali?> ObtenerPorCorreo(string correo)
        {
            return await _UsuarioRepository.ObtenerPorCorreo(correo);
        }

        public async Task GuardarCodigoRecuperacion(string correo, string codigo)
        {
            await _UsuarioRepository.GuardarCodigoRecuperacion(correo, codigo, DateTime.Now.AddMinutes(15));
        }

        public async Task ActualizarContrasena(string correo, string nuevaContrasena)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);
            await _UsuarioRepository.ActualizarContrasena(correo, hash);
        }



        public bool VerificarContrasena(string contrasenaIngresada, string hashAlmacenado)
        {
            return BCrypt.Net.BCrypt.Verify(contrasenaIngresada, hashAlmacenado);
        }

        public async Task EnviarCorreoRecuperacion(string correo, string codigo)
        {
            var mensaje = new MailMessage();
            mensaje.From = new MailAddress("consultoriovinali@gmail.com"); // tu correo
            mensaje.To.Add(correo);
            mensaje.Subject = "Código de recuperación Club Vinali";
            mensaje.Body = $"Tu código de recuperación es: {codigo}";
            mensaje.IsBodyHtml = false;

            using var smtp = new SmtpClient("smtp.gmail.com", 587); // host y puerto SMTP
            smtp.Credentials = new NetworkCredential("consultoriovinali@gmail.com", "nltv jzxl gdad isia");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mensaje);
        }
    }
}
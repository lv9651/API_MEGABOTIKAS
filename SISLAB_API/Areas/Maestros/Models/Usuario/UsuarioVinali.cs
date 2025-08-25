namespace SISLAB_API.Areas.Maestros.Models
{
    public class UsuarioVinali
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;

        // 🚨 Cámbiale el nombre a "Contrasena" (texto plano al recibirlo)
        public string Contrasena { get; set; } = string.Empty;

        // Solo se usa cuando ya está guardado en BD
        public string ContrasenaHash { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; }
        public string? CodigoRecuperacion { get; set; }
        public DateTime? FechaExpiracionCodigo { get; set; }
    }

    public class LoginDto
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }

    public class RecuperarRequest
    {
        public string Correo { get; set; } = string.Empty;
    }

    public class ResetearRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}
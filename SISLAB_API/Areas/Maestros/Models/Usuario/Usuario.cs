namespace SISLAB_API.Areas.Maestros.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }


    public class UsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
    }
}
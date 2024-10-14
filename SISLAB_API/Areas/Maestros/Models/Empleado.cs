namespace SISLAB_API.Areas.Maestros.Models
{
    public class Empleado
    {
        public string id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }

        public string empresa { get; set; }
        public string Cargo { get; set; }
        public string ESTADO { get; set; }


    }
}

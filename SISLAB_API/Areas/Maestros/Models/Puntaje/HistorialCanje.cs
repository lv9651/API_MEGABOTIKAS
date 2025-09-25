namespace SISLAB_API.Areas.Maestros.Models
{
    public class HistorialCanje
    {
        public string Tipo { get; set; }
        public string NombreItem { get; set; }
        public int PuntosDescontados { get; set; }
        public DateTime FechaCanje { get; set; }
        public string Estado { get; set; }
    }
}
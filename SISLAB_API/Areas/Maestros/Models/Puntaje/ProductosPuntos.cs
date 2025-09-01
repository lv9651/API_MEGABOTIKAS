namespace SISLAB_API.Areas.Maestros.Models
{
    public class ProductosPuntos
    {
        public int Codigo { get; set; }
        public string Servicio { get; set; }
        public decimal Puntaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
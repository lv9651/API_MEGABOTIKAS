namespace SISLAB_API.Areas.Maestros.Models
{
    public class HistorialDescuento
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public decimal PuntosTotal { get; set; }
        public decimal PuntosAntesDescuento { get; set; }
        public decimal PuntosDescuento { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public string NivelAnterior { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public string NivelFinal { get; set; }
    }
}
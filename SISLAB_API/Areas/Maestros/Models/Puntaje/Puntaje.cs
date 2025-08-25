namespace SISLAB_API.Areas.Maestros.Models
{
    public class Puntaje
    {
        public DateTime fechacreacion { get; set; }
        public int IdCliente { get; set; }
        public int IdVenta { get; set; }
        public string Sucursal { get; set; }
        public string Serie { get; set; }
        public string NumDocumento { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Puntajes { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
    }
}
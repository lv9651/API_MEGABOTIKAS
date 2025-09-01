namespace SISLAB_API.Areas.Maestros.Models
{
    public class AcumulacionPuntos
    {
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Puntos { get; set; }
        public string Descripcion { get; set; }
        public string Movimiento { get; set; }
        public string Serie { get; set; }
        public string NumDocumento { get; set; }
        public string Sucursal { get; set; }
    }

    public class ProductoCanjeable
    {
        public int Codigo { get; set; }
        public string Servicio { get; set; }
        public decimal Puntaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public bool PuedeCanjear { get; set; }
        public decimal PuntosDisponibles { get; set; }
    }

    public class HistorialCompleto
    {
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Puntos { get; set; }
        public string Descripcion { get; set; }
        public string Movimiento { get; set; }
        public string Serie { get; set; }
        public string NumDocumento { get; set; }
        public string Sucursal { get; set; }
        public string ProductoCanjeado { get; set; }
    }

    public class ResultadoCanje
    {
        public string Mensaje { get; set; }
        public decimal PuntosUtilizados { get; set; }
        public decimal NuevoSaldo { get; set; }
        public string Descripcion { get; set; }
    }

    public class SaldoPuntos
    {
        public decimal SaldoPunto { get; set; }
    }

        public class CanjeRequest
    {
        public int IdUsuario { get; set; }
        public int CodigoProducto { get; set; }

        public string tipomovimiento { get; set; }
    }
}
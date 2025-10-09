namespace SISLAB_API.Areas.Maestros.Models
{
    public class Compra
    {
        public DateTime FecOCompra { get; set; }
        public string LABORATORIO { get; set; }
        public string NroOCompra { get; set; }
        public string Estado_OC { get; set; }
        public string CdArticulo { get; set; }
        public string Nombre_produto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
        public DateTime? FecDocum_Compra { get; set; }
        public string NroDocum_F { get; set; }
        public decimal? MTototal { get; set; }
        public DateTime? FecNCredito { get; set; }
        public string NroNCredito { get; set; }
        public decimal? Monto_NC { get; set; }
        public decimal? Cantidad_NC { get; set; }
        public decimal? Monto_nc_art { get; set; }
        public string DStipoNcredito { get; set; }
        public DateTime? FecPreingreso { get; set; }
        public decimal? CantidadIngresada { get; set; }
        public string lote { get; set; }
        public string fecvto { get; set; }
        public string EstadoCompra { get; set; }
    }
}
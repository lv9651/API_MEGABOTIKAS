namespace SISLAB_API.Areas.Maestros.Models
{
    public class Producto
    {
        public string IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Categoria { get; set; }
        public string Laboratorio { get; set; }
        public string tipo_producto { get; set; }
        public decimal precio_venta { get; set; }
        public decimal precio_costo { get; set; }
        public decimal costo_promedio { get; set; }
        public DateTime UltimaFechaCompra { get; set; }
        public int CantidadUltimaCompra { get; set; }
        public DateTime UltimaFechaIngreso { get; set; }
        public int CantidadUltimaIngreso { get; set; }
        public int StockAlmacen { get; set; }
        public int StockCadena { get; set; }
        public int StockTotal { get; set; }
        public decimal Mes1_Ventas { get; set; }
        public decimal Mes2_Ventas { get; set; }
        public decimal Mes3_Ventas { get; set; }
        public decimal Mes4_Ventas { get; set; }
        public decimal Proyeccion { get; set; }
        public decimal ERI_ALMACEN { get; set; }
        public decimal ERI_CADENA { get; set; }
        public decimal ERI_LOCALES { get; set; }
    }
}
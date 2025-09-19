namespace SISLAB_API.Areas.Maestros.Models
{
    public class PurchaseValidationDto
    {
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public DateTime UltimaFechaCompra { get; set; }   // lo que responde el usuario
        public decimal UltimoMontoCompra { get; set; }    // lo que responde el usuario
    }
}
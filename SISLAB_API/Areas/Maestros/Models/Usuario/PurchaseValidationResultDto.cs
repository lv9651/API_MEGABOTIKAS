namespace SISLAB_API.Areas.Maestros.Models
{
    public class PurchaseValidationResultDto
    {
        public bool IsValid { get; set; }
        public DateTime? DbUltimaFechaCompra { get; set; }
        public decimal? DbMontoTotal { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
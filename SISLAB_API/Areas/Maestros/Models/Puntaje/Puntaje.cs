namespace SISLAB_API.Areas.Maestros.Models
{
    public class ClientePuntosResponse
    {
        public int IdCliente { get; set; }
        public int PuntosTotales { get; set; }
        public int PuntosDisponibles { get; set; }
        public string Mensaje { get; set; }
    }

    // Models/NivelClienteResponse.cs
    public class NivelClienteResponse
    {
        public int IdNivel { get; set; }
        public string Nombre { get; set; }
        public decimal Descuento { get; set; }
        public int PuntosCliente { get; set; }
    }

    // Models/CanjeRequest.cs
    public class CanjeRequest
    {
        public string Correo { get; set; }
        public string Tipo { get; set; } // "PRODUCTO" o "BENEFICIO"
        public string IdReferencia { get; set; } // puede ser idProducto o idBeneficio
  
    }

    // Models/CanjeResponse.cs
    public class CanjeResponse
    {
        public string Mensaje { get; set; }
        public int IdCliente { get; set; }
        public int IdNivel { get; set; }
        public string IdReferencia { get; set; }
        public int PuntosUsados { get; set; }
        public int PuntosRestantes { get; set; }
    }


    public class BeneficioNivel
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public int Puntaje { get; set; }
        public bool Pendiente { get; set; }
    }

    public class ProductoNivel
    {
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Puntaje { get; set; }
        public bool Pendiente { get; set; }
    }

    public class NivelCompletoResponse
    {
        public NivelClienteResponse NivelInfo { get; set; }
        public List<BeneficioNivel> Beneficios { get; set; }
        public List<ProductoNivel> Productos { get; set; }
    }
}
// Models/EmailModel.cs
using Microsoft.AspNetCore.Http;

public class EmailModel
{
    public string Email { get; set; }
    public string Dni { get; set; }
    public string Descripcion { get; set; }
    public string Beneficio { get; set; }
    public IFormFile Document { get; set; } // Agregar esta propiedad
}
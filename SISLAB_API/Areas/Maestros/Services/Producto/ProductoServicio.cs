using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductoServicio
{
    private readonly ProductoRepositorio _productoRepositorio;

    public ProductoServicio(ProductoRepositorio productoRepositorio)
    {
        _productoRepositorio = productoRepositorio;
    }

    // Método para obtener productos con paginación y filtrado
    public async Task<(IEnumerable<Producto>, int)> ObtenerProductosAsync(
         int pagina, int tamanoPagina, string estado, string categoria, string laboratorio,string nombre)
    {
        return await _productoRepositorio.ObtenerProductosAsync(pagina, tamanoPagina, estado, categoria, laboratorio,nombre);
    }

    public async Task<object> ObtenerFiltrosAsync()
    {
        return await _productoRepositorio.ObtenerFiltrosAsync();
    }
}
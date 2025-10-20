using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CompraServicio
{
    private readonly CompraRepository _compraRepositorio;


    public CompraServicio(CompraRepository compraRepositorio)
    {
        _compraRepositorio = compraRepositorio;
    }

    // Método para obtener productos con paginación y filtrado
    public async Task<(IEnumerable<Compra> Compras, int TotalCount)> BuscarCompras(
            string? nroOCompra,
            string? nroFactura,
            string? cdArticulo,
            string? nombreProducto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? estadoOC,
             string? Empresa,
            string? Comprador,
            string? aprobacioN_OC,
            int page,
            int pageSize)
    {
        return await _compraRepositorio.BuscarCompras(
            nroOCompra, nroFactura, cdArticulo, nombreProducto,
            fechaInicio, fechaFin, estadoOC,Empresa,Comprador, aprobacioN_OC, page, pageSize
        );
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CompraController : ControllerBase
{
    private readonly CompraServicio _compraServicio;

    public CompraController(CompraServicio productoServicio)
    {
        _compraServicio = productoServicio;
    }

    // Endpoint para obtener productos con paginación y filtrado


    [HttpGet("Buscar")]
    public async Task<IActionResult> BuscarCompras(
       [FromQuery] string? nroOCompra,
       [FromQuery] string? nroFactura,
       [FromQuery] string? cdArticulo,
       [FromQuery] string? nombreProducto,
       [FromQuery] DateTime? fechaInicio,
       [FromQuery] DateTime? fechaFin,
       [FromQuery] string? estadoOC,
       [FromQuery] bool exportarTodo = false,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 50)
    {
        if (exportarTodo)
        {
            // 🔸 Ignora paginación y devuelve todo
            var (compras, total) = await _compraServicio.BuscarCompras(
                nroOCompra, nroFactura, cdArticulo, nombreProducto,
                fechaInicio, fechaFin, estadoOC, 1, int.MaxValue
            );

            return Ok(new
            {
                page = 1,
                pageSize = total,
                total,
                totalPages = 1,
                data = compras
            });
        }

        // 🔹 Caso normal con paginación
        var (comprasPaginadas, totalFilas) = await _compraServicio.BuscarCompras(
            nroOCompra, nroFactura, cdArticulo, nombreProducto,
            fechaInicio, fechaFin, estadoOC, page, pageSize
        );

        return Ok(new
        {
            page,
            pageSize,
            total = totalFilas,
            totalPages = (int)Math.Ceiling((double)totalFilas / pageSize),
            data = comprasPaginadas
        });
    }

}


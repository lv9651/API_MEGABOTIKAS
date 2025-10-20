using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly ProductoServicio _productoServicio;

    public ProductoController(ProductoServicio productoServicio)
    {
        _productoServicio = productoServicio;
    }

    // Endpoint para obtener productos con paginación y filtrado


        // 🔹 GET con filtros y paginación
        [HttpGet("obtenerquiebre")]
        public async Task<ActionResult> Get(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 10,
            [FromQuery] string estado = null,
            [FromQuery] string categoria = null,
            [FromQuery] string laboratorio = null,
               [FromQuery] string Nombre = null)
        {
            try
            {
                var (productos, total) = await _productoServicio.ObtenerProductosAsync(pagina, tamanoPagina, estado, categoria, laboratorio,Nombre);
                int totalPaginas = (int)Math.Ceiling((double)total / tamanoPagina);

                return Ok(new
                {
                    total,
                    totalPaginas,
                    paginaActual = pagina,
                    productos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    [HttpGet("rotacion-local/{codigoProducto}")]
    public async Task<IActionResult> GetRotacionPorLocal(string codigoProducto)
    {
        var data = await _productoServicio.GetRotacionPorLocal(codigoProducto);
        return Ok(data);
    }
    // 🔹 GET filtros para combobox
    [HttpGet("filtros")]
        public async Task<ActionResult> GetFiltros()
        {
            try
            {
                var filtros = await _productoServicio.ObtenerFiltrosAsync();
                return Ok(filtros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

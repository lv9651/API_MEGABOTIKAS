using API_MEGABOTIKAS.Areas.Maestros.Models.Almacen;
using API_MEGABOTIKAS.Areas.Maestros.Services.Almacen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SISLAB_API.Areas.Maestros.Services;

namespace API_MEGABOTIKAS.Areas.Maestros.Controllers.Almacen
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlmacenController : Controller
    {
        private readonly AlmacenServicio almacenService;
       
        public AlmacenController(AlmacenServicio _almacenServicio)
        {
            almacenService = _almacenServicio;
        }

        [HttpGet("obtenerStock_X_Local")]
        public async Task<IActionResult> obtenerStock_X_Local(int codArticulo)
        {
            var stock = await almacenService.obtenerStock_X_Local(codArticulo);

            if(stock == null || !stock.Any())
            {
                return NotFound("No se encontro stock para este articulo");
            }

            return Ok(stock);
        }
    }
}

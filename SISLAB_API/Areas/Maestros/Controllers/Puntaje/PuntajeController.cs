using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;
using System.Web;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity.Data;
using System.Runtime.InteropServices.ObjectiveC;




namespace SISLAB_API.Areas.Maestros.Controllers // Cambia esto al espacio de nombres real
{

    [Route("api/[controller]")]
[ApiController]
public class  PuntajeController : ControllerBase
{
        private readonly PuntajeServicio _servicio;
        private readonly Object _Logger;
        public PuntajeController(PuntajeServicio servicio)
        {
            _servicio = servicio;
        }




   

        [HttpPost("actualizar-puntos")]
        public async Task<IActionResult> ActualizarPuntos([FromQuery] string correo)
        {
            var result = await _servicio.ActualizarPuntosCliente(correo);
            return Ok(result);
        }

        [HttpGet("nivel-completo")]
        public async Task<IActionResult> ObtenerNivelCompleto([FromQuery] string correo)
        {
            var nivel = await _servicio.ObtenerNivelCliente(correo);
            if (nivel == null)
                return NotFound("No se pudo determinar nivel");

            return Ok(nivel);
        }


        [HttpGet("historial/{idCliente}")]
        public async Task<IActionResult> ObtenerHistorial(int idCliente)
        {
            var historial = await _servicio.ObtenerHistorialAsync(idCliente);

            if (historial == null)
                return NotFound(new { mensaje = "No se encontraron canjes para este cliente." });

            return Ok(historial);
        }
        [HttpPost("canjear")]
        public async Task<IActionResult> Canjear([FromBody] CanjeRequest request)
        {
            var result = await _servicio.ProcesarCanje(request);
            return Ok(result);
        }



        // Endpoint para obtener el historial de descuentos de un cliente



    }


}
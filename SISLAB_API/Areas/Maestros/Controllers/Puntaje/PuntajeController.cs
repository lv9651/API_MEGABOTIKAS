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




namespace SISLAB_API.Areas.Maestros.Controllers // Cambia esto al espacio de nombres real
{

    [Route("api/[controller]")]
[ApiController]
public class  PuntajeController : ControllerBase
{
        private readonly PuntajeServicio _servicio;

        public PuntajeController(PuntajeServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet("puntos/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Puntaje>>> ObtenerVentasPuntos(int idUsuario)
        {
            var ventas = await _servicio.ObtenerVentasPuntosAsync(idUsuario);

            if (ventas == null || !ventas.Any())
            {
                return NotFound("No se encontraron ventas para el usuario especificado.");
            }

            return Ok(ventas);
        }







        [HttpPost]
        [Route("insertar")]
        public IActionResult InsertarHistorial([FromBody] HistorialDescuento historialDescuento)
        {
            if (historialDescuento == null)
            {
                return BadRequest("Los datos del historial son inválidos.");
            }

            try
            {
                // Llamamos al servicio para insertar el historial
                _servicio.InsertarHistorialDescuento(
                    historialDescuento.IdCliente,
                    historialDescuento.PuntosTotal,
                    historialDescuento.PuntosAntesDescuento,
                    historialDescuento.PuntosDescuento,
                    historialDescuento.NivelAnterior,
                    historialDescuento.DescuentoAplicado,
                    historialDescuento.NivelFinal
                );
                return Ok("Historial de descuento insertado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Endpoint para obtener el historial de descuentos de un cliente
        [HttpGet]
        [Route("cliente/{idCliente}")]
        public IActionResult ObtenerHistorialPorCliente(int idCliente)
        {
            try
            {
                var historial = _servicio.ObtenerHistorialPorCliente(idCliente);
                if (historial == null || !historial.Any())
                {
                    return NotFound("No se encontraron registros de descuentos para este cliente.");
                }

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


    }


}
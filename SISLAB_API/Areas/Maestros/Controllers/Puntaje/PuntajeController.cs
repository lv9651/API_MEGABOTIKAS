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

  

        [HttpGet("obtenerProductos")]
        public async Task<ActionResult<IEnumerable<PuntajeServicio>>> Get()
        {
            try
            {
                var servicios = await _servicio.ObtenerTodosLosServicios();
                return Ok(servicios);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener servicios" });
            }
        }


        [HttpGet("ventas-puntos/{idUsuario}")]
        public async Task<IActionResult> GetVentasPuntos(int idUsuario)
        {
            try
            {
                var resultados = await _servicio.ObtenerVentasPuntos(idUsuario);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("productos-canjeables")]
        public async Task<IActionResult> GetProductosCanjeables()
        {
            try
            {
                var resultados = await _servicio.ObtenerProductosCanjeables(null);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("productos-canjeables/{idUsuario}")]
        public async Task<IActionResult> GetProductosCanjeables(int idUsuario)
        {
            try
            {
                var resultados = await _servicio.ObtenerProductosCanjeables(idUsuario);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpPost("canjear-producto")]
        public async Task<IActionResult> CanjearProducto([FromBody] CanjeRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Solicitud inválida" });

                var resultado = await _servicio.CanjearProducto(request.IdUsuario, request.CodigoProducto, request.tipomovimiento);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("saldo-puntos/{idUsuario}")]
        public async Task<IActionResult> GetSaldoPuntos(int idUsuario)
        {
            try
            {
                var saldo = await _servicio.ObtenerSaldoPuntosModel(idUsuario);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("historial-completo/{idUsuario}")]
        public async Task<IActionResult> GetHistorialCompleto(int idUsuario)
        {
            try
            {
                var resultados = await _servicio.ObtenerHistorialCompleto(idUsuario);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        
        // Endpoint para obtener el historial de descuentos de un cliente
      


    }


}
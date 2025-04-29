using Microsoft.AspNetCore.Mvc;
using VideoReportApi.Services;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoReportController : ControllerBase
    {
        private readonly VideoReportService _videoReportService;

        public VideoReportController(VideoReportService videoReportService)
        {
            _videoReportService = videoReportService;
        }

        // Endpoint para obtener reportes filtrados
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ReporteVideo>>> GetFilteredReports(
            [FromQuery] string dni = null,
            [FromQuery] string nombre = null,
            [FromQuery] string videoStatus = null)
        {
            // Llamar al servicio para obtener los reportes con los parámetros opcionales
            var reports = await _videoReportService.GetFilteredReportsAsync(dni, nombre, videoStatus);
            return Ok(reports);
        }
    }
}
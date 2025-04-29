using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SISLAB_API.Areas.Maestros.Models;
using VideoReportApi.Repositories;

namespace VideoReportApi.Services
{
    public class VideoReportService
    {
        private readonly VideoReportRepository _videoReportRepository;

        public VideoReportService(VideoReportRepository videoReportRepository)
        {
            _videoReportRepository = videoReportRepository;
        }

        // Llamar al repositorio para obtener los reportes filtrados de forma asincrónica
        public async Task<IEnumerable<ReporteVideo>> GetFilteredReportsAsync(string dni = "", string nombre = "", string videoStatus = "")
        {
            return await _videoReportRepository.GetFilteredReportsAsync(dni, nombre, videoStatus);
        }
    }
}
using System.IO;
using System.Threading.Tasks;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class VideoService
    {
        private readonly string _videoDirectory;

        public VideoService(string videoDirectory)
        {
            _videoDirectory = videoDirectory;
        }

        public async Task<string> GetVideoPathByIdAsync(string id)
        {
            // Generar el nombre del archivo usando el ID
            string videoFileName = $"{id}.mp4"; // Cambia la extensión a mp4
            string videoPath = Path.Combine(_videoDirectory, videoFileName);

            // Verifica si el archivo existe y devuelve la ruta
            return File.Exists(videoPath) ? videoPath : null;
        }
    }
}
using System.IO;
using System.Threading.Tasks;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class ImageService
    {
        private readonly string _imageDirectory;

        public ImageService(string imageDirectory)
        {
            _imageDirectory = imageDirectory;
        }

        public async Task<string> GetImagePathByIdAsync(string id)
        {
            // Generar el nombre del archivo usando el ID
            string imageFileName = $"{id}.jpg"; // Cambia la extensión si es necesario
            string imagePath = Path.Combine(_imageDirectory, imageFileName);

            // Verifica si el archivo existe y devuelve la ruta
            return File.Exists(imagePath) ? imagePath : null;
        }
    }
}
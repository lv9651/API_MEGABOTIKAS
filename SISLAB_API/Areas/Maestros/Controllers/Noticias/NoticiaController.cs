using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SISLAB_API.Areas.Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly NoticiaService _noticiaService;

        public NoticiaController(NoticiaService NoticiaService)
        {
            _noticiaService = NoticiaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Noticias>>> GetAllNewsAsync()
        {
            var noticias = await _noticiaService.GetAllNewsAsync();
            return Ok(noticias);
        }




        [HttpPost("upload")]
        public async Task<IActionResult> UploadNews(
            [FromForm] string title,
            [FromForm] string content,
            ICollection<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("Se deben proporcionar imágenes.");
            }

            var fileStreams = new Dictionary<string, Stream>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var stream = new MemoryStream();
                    await image.CopyToAsync(stream);
                    stream.Position = 0; // Reiniciar el stream para lectura posterior
                    fileStreams.Add(image.FileName, stream);
                }
            }

            try
            {
                foreach (var image in fileStreams)
                {
                    // Generar un nombre único o ruta para guardar la imagen
                    string uniqueFileName = $"{Guid.NewGuid()}_{image.Key}";
                    string imageUrl = Path.Combine(@"\\PANDAFILE\Intranet\Img", uniqueFileName);

                    using (var fileStream = new FileStream(imageUrl, FileMode.Create))
                    {
                        await image.Value.CopyToAsync(fileStream);
                    }

                    // Guarda la noticia, asignando la URL de la imagen
                    await _noticiaService.SaveNewsAsync(new InsertNoticia
                    {
                        Title = title,
                        content = content,
                        image_url = uniqueFileName // Asigna el nombre del archivo
                    });
                }

                return Ok("Noticias cargadas exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar las imágenes: {ex.Message}");
            }
            finally
            {
                foreach (var stream in fileStreams.Values)
                {
                    stream.Dispose();
                }
            }


        }

        [HttpGet("image/{id}")]
        public IActionResult GetImageById(string id)
        {

            try
            {
                // Generar el nombre del archivo usando el ID
                string imageFileName = $"{id}"; // Cambia la extensión si es necesario
                string imagePath = Path.Combine(@"\\PANDAFILE\Intranet\Img", imageFileName);
                Console.WriteLine($"Attempting to retrieve image: {imageFileName}");
                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound("Imagen no encontrada.");
                }

                var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/jpeg"); // Cambia el tipo MIME si es necesario
            }
            catch (Exception ex)
            {
                // Devuelve el mensaje de error específico
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsByIdAsync(int id)
        {
            try
            {
                var deleted = await _noticiaService.DeleteNewsByIdAsync(id);
                if (!deleted)
                {
                    return NotFound("Noticia no encontrada.");
                }
                return NoContent(); // Devuelve un 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la noticia: {ex.Message}");
            }
        }



    }
}





























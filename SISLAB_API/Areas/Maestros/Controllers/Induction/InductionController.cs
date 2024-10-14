using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SISLAB_API.Areas.Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InductionController : ControllerBase
    {
        private readonly InductionService _inductionservice;


        public InductionController(InductionService InductionService)
        {
            _inductionservice = InductionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Induction>>> GetAllInductionAsync()
        {
            var noticias = await _inductionservice.GetAllInductionAsync();
            return Ok(noticias);
        }


        [HttpPost("upload-videos")]
        public async Task<IActionResult> UploadVideos(
               [FromForm] int id,
    [FromForm] string title,
    [FromForm] string content,
    [FromForm] string module,
    ICollection<IFormFile> videos)
        {
            if (videos == null || videos.Count == 0)
            {
                return BadRequest("Se deben proporcionar videos.");
            }

            var fileStreams = new Dictionary<string, Stream>();

            foreach (var video in videos)
            {
                if (video.Length > 0)
                {
                    var stream = new MemoryStream();
                    await video.CopyToAsync(stream);
                    stream.Position = 0; // Reiniciar el stream para lectura posterior
                    fileStreams.Add(video.FileName, stream);
                }
            }

            try
            {
                foreach (var video in fileStreams)
                {
                    // Generar un nombre único o ruta para guardar el video
                    string uniqueFileName = $"{Guid.NewGuid()}_{video.Key}";
                    string videoUrl = Path.Combine(@"\\192.168.154.12\fileserver\TI\Velasquez\Videos", uniqueFileName);

                    using (var fileStream = new FileStream(videoUrl, FileMode.Create))
                    {
                        await video.Value.CopyToAsync(fileStream);
                    }

                    // Guarda la noticia, asignando la URL del video
                    await _inductionservice.SaveInductionAsync(new Induction
                    {
                        id = id,
                        title = title,
                        content = content,
                        video_url = uniqueFileName ,// Asigna el nombre del archivo (podrías cambiar esto a video_url si es necesario)
                        module=module
                    });
                }

                return Ok("Videos cargados exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar los videos: {ex.Message}");
            }
            finally
            {
                foreach (var stream in fileStreams.Values)
                {
                    stream.Dispose();
                }
            }
        }




        [HttpGet("video/{id}")]
        public IActionResult GetVideoById(string id)
        {
            // Generar el nombre del archivo usando el ID con extensión .mp4
            string videoFileName = $"{id}"; // Cambia la extensión a mp4
            string videoPath = Path.Combine(@"\\192.168.154.12\fileserver\TI\Velasquez\Videos", videoFileName);

            if (!System.IO.File.Exists(videoPath))
            {
                return NotFound("Video no encontrado.");
            }

            var fileStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "video/mp4"); // Cambia el tipo MIME a video/mp4
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsByIdAsync(int id)
        {
            try
            {
                var deleted = await _inductionservice.DeleteNewsByIdAsync(id);
                if (!deleted)
                {
                    return NotFound("Video no encontrada.");
                }
                return NoContent(); // Devuelve un 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la noticia: {ex.Message}");
            }
        }




        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CommentR comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.comment))
            {
                return BadRequest("Invalid comment data.");
            }

            try
            {
                await _inductionservice.AddCommentAsync(id, comment);
                return StatusCode(201, new { message = "Comment added successfully" });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { message = $"Error adding comment: {ex.Message}" });
            }
        }


        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            try
            {
                var comments = await _inductionservice.GetCommentsForVideoAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching comments: {ex.Message}");
                return StatusCode(500, "Error fetching comments");
            }
        }


        [HttpGet("{userId}/video-progress")]
        public async Task<ActionResult<IEnumerable<VideoProgress>>> GetVideoProgress(string userId, [FromQuery] string module)
        {
            try
            {
                var results = await _inductionservice.GetVideoProgressAsync(userId, module);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener el progreso de los videos: {ex.Message}");
                return StatusCode(500, new { error = "Error al obtener el progreso de los videos" });
            }
        }






        /*    [HttpPost("{id}/video-progress")]
            public async Task<IActionResult> AddVideoProgress(string userId, [FromBody] VideoProgressRequest request)
            {
                string videoTitle = string.Empty;

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Primero, obtén el título del video
                    using (var command = new SqlCommand("SELECT title FROM induction_videos WHERE id = @videoId", connection))
                    {
                        command.Parameters.AddWithValue("@videoId", request.VideoId);
                        var result = await command.ExecuteScalarAsync();

                        if (result == null)
                        {
                            return NotFound(new { error = "Video no encontrado" });
                        }

                        videoTitle = result.ToString();
                    }

                    // Agrega el progreso del video
                    using (var insertProgressCommand = new SqlCommand("INSERT INTO user_video_progress (user_id, video_id, watched) VALUES (@userId, @videoId, @watched)", connection))
                    {
                        insertProgressCommand.Parameters.AddWithValue("@userId", userId);
                        insertProgressCommand.Parameters.AddWithValue("@videoId", request.VideoId);
                        insertProgressCommand.Parameters.AddWithValue("@watched", true);

                        await insertProgressCommand.ExecuteNonQueryAsync();
                    }

                    // Agrega la notificación
                    var notificationMessage = $"Has visto el video: {videoTitle}";
                    using (var insertNotificationCommand = new SqlCommand("INSERT INTO notifications (user_id, message) VALUES (@userId, @message)", connection))
                    {
                        insertNotificationCommand.Parameters.AddWithValue("@userId", userId);
                        insertNotificationCommand.Parameters.AddWithValue("@message", notificationMessage);

                        await insertNotificationCommand.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Progreso del video y notificación añadidos exitosamente" });
            }
        }


            */





    }



}






























using SISLAB_API.Areas.Maestros.Models;


namespace SISLAB_API.Areas.Maestros.Services
{
    public class NoticiaService

    {
        private readonly NoticiaRepository _NoticiaRepository;
        private readonly ImageService _imageService;

        public NoticiaService(NoticiaRepository noticiaRepository)
        {
            _NoticiaRepository = noticiaRepository;
        }

        public async Task<IEnumerable<Noticias>> GetAllNewsAsync()
        {
            return await _NoticiaRepository.GetAllNewsAsync();
        }


        public async Task SaveNewsAsync(InsertNoticia noticia)
        {
            await _NoticiaRepository.InsertNewsAsync(noticia);
        }


        public async Task<string> GetImagePathByIdAsync(string id)
        {
            return await _imageService.GetImagePathByIdAsync(id);
        }


        public async Task<bool> DeleteNewsByIdAsync(int id)
        {
            return await _NoticiaRepository.DeleteNewsByIdAsync(id);
        }




        public async Task<NoticiasAct> ActualizarNoticiaAsync(NoticiasAct noticia)
        {
            // Llamamos directamente al repositorio para realizar la actualización
            var updated = await _NoticiaRepository.UpdateNoticiaAsync(noticia);

            return updated ? noticia : null; // Si la actualización tuvo éxito, retornamos la noticia
        }


    }
}

namespace SISLAB_API.Areas.Maestros.Models
{
    public class VideoProgress
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public string Module { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

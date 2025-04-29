namespace SISLAB_API.Areas.Maestros.Models
{
    public class Noticias
    {
        public int id { get; set; }
        public string title  { get; set; }
        public string content { get; set; }

        public string image_url { get; set; }

        public string bloque { get; set; }


    }



    public class NoticiasAct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Bloque { get; set; }
    }
}

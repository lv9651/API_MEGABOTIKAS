namespace SISLAB_API.Areas.Maestros.Models
{
    public class AgendaItem
    {
        public int Id { get; set; }  // Unique identifier
        public string MeetingName { get; set; }  // Name of the meeting
        public string Dni { get; set; }  // DNI of the creator
        public string Date { get; set; }  // Date of the meeting
        public TimeSpan Time { get; set; }  // Time of the meeting
    }
}
using FileToEmailLinker.Models.Enums;

namespace FileToEmailLinker.Models.Entities
{
    public class Alert
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string? References { get; set; }
        public AlertSeverity AlertSeverity { get; set; }
        public bool Visualized { get; set; }
    }
}

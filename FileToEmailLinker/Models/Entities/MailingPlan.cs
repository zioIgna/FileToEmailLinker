using FileToEmailLinker.Models.Enums;

namespace FileToEmailLinker.Models.Entities
{
    public class MailingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ActiveState ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public ICollection<string> FileNames { get; set; } = new List<string>();
        public ICollection<Receiver> ReceiverList { get; set; } = new List<Receiver>();
        public int SchedulationId { get; set; }
        public Schedulation? Schedulation { get; set; }
    }
}

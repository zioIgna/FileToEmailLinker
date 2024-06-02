using FileToEmailLinker.Models.Enums;
using FileToEmailLinker.Models.ViewModels;

namespace FileToEmailLinker.Models.Entities
{
    public class MailingPlan : IIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ActiveState ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string FileStringList { get; set; }
        public ICollection<Receiver> ReceiverList { get; set; } = new List<Receiver>();
        public WeeklySchedulation? WeeklySchedulation { get; set; }
        public MonthlySchedulation? MonthlySchedulation { get; set; }

        public Schedulation? Schedulation { get => WeeklySchedulation != null ? WeeklySchedulation : MonthlySchedulation != null ? MonthlySchedulation : null; }
    }
}

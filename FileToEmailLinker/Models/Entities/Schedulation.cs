using FileToEmailLinker.Models.Enums;
using NuGet.Protocol;

namespace FileToEmailLinker.Models.Entities
{
    public class Schedulation
    {
        public int Id { get; set; }
        public SchedulingRecurrence Recurrence { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}

using FileToEmailLinker.Models.Enums;

namespace FileToEmailLinker.Models.Entities
{
    public class Schedulation
    {
        public int Id { get; set; }
        public string Name { get; set; } = DateTime.Now.ToString();
        public SchedulingRecurrence Recurrence { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly EndDate { get; set; } = DateOnly.MaxValue;
        ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}

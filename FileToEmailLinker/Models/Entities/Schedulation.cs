using FileToEmailLinker.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.Entities
{
    public class Schedulation : ISchedulation
    {
        public int Id { get; set; }
        //public string Name { get; set; } // = DateTime.Now.ToString();
        //public SchedulingRecurrence Recurrence { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly? Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly EndDate { get; set; } = DateOnly.MaxValue;
        public int MailingPlanId { get; set; }
        public MailingPlan? MailingPlan { get; set; }
    }
}

using FileToEmailLinker.Models.Enums;

namespace FileToEmailLinker.Models.Entities
{
    public class MailingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ActiveState ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string FileStringList { get; set; }
        //public ICollection<FileRef> FileNames { get; set; } = new List<FileRef>();
        public ICollection<Receiver> ReceiverList { get; set; } = new List<Receiver>();
        //public int SchedulationId { get; set; }
        public WeeklySchedulation? WeeklySchedulation { get; set; }
        public MonthlySchedulation? MonthlySchedulation { get; set; }
        public FixedDatesSchedulation? FixedDatesSchedulation { get; set; }

        //public Schedulation? Schedulation { get; set; }

        //public string[] GetReceiverListArray()
        //{
        //    var list = new List<string>();
        //    if(ReceiverList != null && ReceiverList.Count > 0)
        //    {
        //        list.AddRange(ReceiverList.Select(elem => elem.Email));
        //    }
        //    return list.ToArray();
        //}
    }
}

namespace FileToEmailLinker.Models.Entities
{
    public class FileRef
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}

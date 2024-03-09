using FileToEmailLinker.Models.ViewModels;

namespace FileToEmailLinker.Models.Entities
{
    public class Receiver : IIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}

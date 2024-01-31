namespace FileToEmailLinker.Models.Entities
{
    public interface ISchedulation
    {
        DateOnly? Date { get; set; }
        DateOnly EndDate { get; set; }
        int Id { get; set; }
        MailingPlan MailingPlan { get; set; }
        int MailingPlanId { get; set; }
        DateOnly StartDate { get; set; }
        TimeOnly Time { get; set; }
    }
}
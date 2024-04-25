namespace FileToEmailLinker.Models.Services.Schedulation
{
    public interface ISchedulationService
    {
        Task<ICollection<Entities.Schedulation>> GetActiveSchedulationsByDateOrWeekDay(DateOnly date);
    }
}
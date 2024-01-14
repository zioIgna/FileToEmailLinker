namespace FileToEmailLinker.Models.Services.Receiver
{
    public interface IReceiverService
    {
        Task<ICollection<Entities.Receiver>> GetReceiverListAsync();
    }
}
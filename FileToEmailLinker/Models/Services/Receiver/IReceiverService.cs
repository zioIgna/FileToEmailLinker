namespace FileToEmailLinker.Models.Services.Receiver
{
    public interface IReceiverService
    {
        Task<ICollection<Entities.Receiver>> GetReceiverListAsync();
        Task<Entities.Receiver> GetReceiverByIdAsync(int id);
        Task<Entities.Receiver> CreateReceiverAsync(Entities.Receiver receiver);
    }
}
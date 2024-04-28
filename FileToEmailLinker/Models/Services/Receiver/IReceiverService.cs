using FileToEmailLinker.Models.ViewModels;

namespace FileToEmailLinker.Models.Services.Receiver
{
    public interface IReceiverService
    {
        Task<ListViewModel<Entities.Receiver>> GetReceiverListForViewModelAsync(int page, int limit, string search);
        Task<ICollection<Entities.Receiver>> GetReceiverListAsync();
        Task<Entities.Receiver> GetReceiverByIdAsync(int id);
        Task<Entities.Receiver> CreateReceiverAsync(Entities.Receiver receiver);
        Task<ReceiverListViewModel> GetReceiverListViewModelAsync(int page, int limit, string search);
    }
}
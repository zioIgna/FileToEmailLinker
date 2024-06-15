using FileToEmailLinker.Models.InputModels.Attachments;

namespace FileToEmailLinker.Models.Services.Attachment
{
    public interface IAttachmentService
    {
        string GetFilesDirectoryFullPath();
        IEnumerable<string> GetFolderFiles();
        Task<ICollection<AttachmentInfo>> GetAttachments();
    }
}
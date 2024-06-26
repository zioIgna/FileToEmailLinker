﻿using FileToEmailLinker.Models.InputModels.Attachments;

namespace FileToEmailLinker.Models.Services.Attachment
{
    public interface IAttachmentService
    {
        string GetFilesDirectoryFullPath();
        IEnumerable<string> GetFolderFiles();
        Task<ICollection<AttachmentInfo>> GetAttachments();
        Task<bool> FileAlreadyExists(IFormFile attachment);
        void UploadFile(IFormFile attachment);
        void UploadFileToTempDir(IFormFile attachment);
        void MoveAttachmentFromTempFolder();
        void DeleteTempAttachment();
        string DeleteAttachment(string fileName);
    }
}
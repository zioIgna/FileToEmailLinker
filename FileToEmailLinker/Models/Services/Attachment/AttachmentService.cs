using FileToEmailLinker.Models.InputModels.Attachments;
using FileToEmailLinker.Models.Services.MailingPlan;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Specialized;

namespace FileToEmailLinker.Models.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public AttachmentService(IConfiguration configuration, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory)
        {
            this.configuration = configuration;
            this.env = env;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<string> GetFolderFiles()
        {
            string fullPath = GetFilesDirectoryFullPath();
            IEnumerable<string>? files = Directory.EnumerateFiles(fullPath);
            files = FilterFilesByExtension(files);
            return files;
        }

        private IEnumerable<string> FilterFilesByExtension(IEnumerable<string> files)
        {
            string[] extensions = configuration.GetSection("AttachmentOptions").GetSection("AcceptedExtensions").Get<string[]>();
            if (extensions != null && extensions.Length > 0)
            {
                files = files.Where(file => extensions.Any(ext => file.EndsWith(ext)));
            }
            return files;
        }

        public async Task<ICollection<AttachmentInfo>> GetAttachments()
        {
            var files = GetFolderFiles();
            ICollection<AttachmentInfo> attachmentInfoList = new List<AttachmentInfo>();
            foreach (var file in files)
            {
                AttachmentInfo attachmentInfo = new();
                attachmentInfo.Name = Path.GetFileName(file);
                using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                IMailingPlanService mailingPlanService = serviceProvider.GetRequiredService<IMailingPlanService>();
                attachmentInfo.MailingPlanList.AddRange((await mailingPlanService.GetAllMailinPlanListAsync()).Where(mp => mp.FileStringList.Split(';').Any(fn => fn.Equals(attachmentInfo.Name))));

                attachmentInfoList.Add(attachmentInfo);
            }

            return attachmentInfoList;
        }

        public string GetFilesDirectoryFullPath()
        {
            var rootdir = env.ContentRootPath;
            var folderPath = configuration["HolderPath"];
            if (folderPath == null)
            {
                throw new Exception("La cartella degli allegati non è raggiungibile");
            }
            var fullPath = Path.Combine(rootdir, "wwwroot", folderPath);
            return fullPath;
        }

        public string GetTempFilesDirectoryFullPath()
        {
            var rootdir = env.ContentRootPath;
            var folderPath = configuration["TempFolderPath"];
            if (folderPath == null)
            {
                throw new Exception("La cartella degli allegati non è raggiungibile");
            }
            var fullPath = Path.Combine(rootdir, "wwwroot", folderPath);
            return fullPath;
        }

        public async Task<bool> FileAlreadyExists(IFormFile attachment)
        {
            ICollection<AttachmentInfo> attachments = await GetAttachments();
            return attachments.Any(att => att.Name.Equals(attachment.FileName));
        }

        public void UploadFile(IFormFile attachment)
        {
            var fileName = Path.GetFileName(attachment.FileName);
            var filesDirectoryFullPath = GetFilesDirectoryFullPath();
            Directory.CreateDirectory(filesDirectoryFullPath);
            var filePath = Path.Combine(filesDirectoryFullPath, fileName);
            using (var localFile = File.OpenWrite(filePath))
            using (var uploadedFile = attachment.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
        }

        public void UploadFileToTempDir(IFormFile attachment)
        {
            try
            {
                var tempFilesDirectory = GetTempFilesDirectoryFullPath();
                if (Directory.Exists(tempFilesDirectory))
                {
                    var fileEntries = Directory.GetFiles(tempFilesDirectory);
                    foreach (var file in fileEntries)
                    {
                        File.Delete(file);
                    }
                }
                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(tempFilesDirectory, fileName);
                Directory.CreateDirectory(tempFilesDirectory);
                using (var localFile = File.OpenWrite(filePath))
                using (var uploadedFile = attachment.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Non è stato possibile caricare il file. {ex.Message}");
            }
        }

        public void MoveAttachmentFromTempFolder()
        {
            string tempFolderPath = GetTempFilesDirectoryFullPath();
            Directory.CreateDirectory(tempFolderPath);
            string destinationFolderPath = GetFilesDirectoryFullPath();
            var fileEntries = Directory.GetFiles(tempFolderPath);
            if (fileEntries == null || fileEntries.Count() == 0)
            {
                throw new Exception("Non è stato trovato il file da caricare. Riprovare");
            }
            if (fileEntries.Count() > 1)
            {
                foreach (var file in fileEntries)
                {
                    File.Delete(file);
                }
                throw new Exception("Sono stati trovati più files nella directory temporanea. Riprovare il caricamento");
            }
            string fileName = Path.GetFileName(fileEntries[0]);
            string fileSource = Path.Combine(tempFolderPath, fileName);
            string fileDestination = Path.Combine(destinationFolderPath, fileName);
            if (File.Exists(fileDestination))
            {
                File.Delete(fileDestination);
            }
            File.Move(fileSource, fileDestination);
        }

        public void DeleteTempAttachment()
        {
            try
            {
                string tempFolderPath = GetTempFilesDirectoryFullPath();
                if (Directory.Exists(tempFolderPath))
                {
                    var fileEntries = Directory.GetFiles(tempFolderPath);
                    if (fileEntries != null && fileEntries.Count() > 0)
                    {
                        foreach (var file in fileEntries)
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Non è stato possibile caricare il file. {ex.Message}");
            }
            
        }

        public string DeleteAttachment(string fileName)
        {
            string fileNameWPath = Path.Combine(GetFilesDirectoryFullPath(), fileName);
            try
            {
                File.Delete(fileNameWPath);
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return string.Empty;
        }
    }
}

using FileToEmailLinker.Models.InputModels.Attachments;
using FileToEmailLinker.Models.Services.MailingPlan;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace FileToEmailLinker.Models.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;
        private readonly IMailingPlanService mailingPlanService;

        public AttachmentService(IConfiguration configuration, IWebHostEnvironment env, IMailingPlanService mailingPlanService)
        {
            this.configuration = configuration;
            this.env = env;
            this.mailingPlanService = mailingPlanService;
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
            //int[] values = configuration.GetSection("DropdownOptions").GetSection("MailingPlan").Get<int[]>();
            string[] extensions = configuration.GetSection("AttachmentOptions").GetSection("AcceptedExtensions").Get<string[]>();
            if (extensions != null && extensions.Length > 0)
            {
                files = files.Where(file => extensions.Any(ext => file.EndsWith(ext))); // file.EndsWith(".xlsx") || file.EndsWith(".xls")).Select(file => Path.GetFileName(file));
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
                //var isAttached = (await mailingPlanService.GetAllMailingPlanListAsync().ToListAsync()).Any(mp => mp.FileStringList.Contains(fileName));
                //attachmentInfo.MailingPlanList.AddRange((await mailingPlanService.GetAllMailingPlanListQuery().ToListAsync()).Where(mp => mp.FileStringList.Split(';').Any(fn => fn.Equals(file))));
                attachmentInfo.MailingPlanList.AddRange((await mailingPlanService.GetAllMailinPlanListAsync()).Where(mp => mp.FileStringList.Split(';').Any(fn => fn.Equals(attachmentInfo.Name))));
                //attachmentInfo.IsDeletable = !isAttached;

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

        public async Task<bool> FileAlreadyExists(IFormFile attachment)
        {
            ICollection<AttachmentInfo> attachments = await GetAttachments();
            return attachments.Any(att => att.Name.Equals(attachment.FileName));
        }

        public void UploadFile(IFormFile attachment)
        {
            var fileName = Path.GetFileName(attachment.FileName);
            var filePath = Path.Combine(GetFilesDirectoryFullPath(), fileName);
            using (var localFile = File.OpenWrite(filePath))
            using (var uploadedFile = attachment.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
        }
    }
}

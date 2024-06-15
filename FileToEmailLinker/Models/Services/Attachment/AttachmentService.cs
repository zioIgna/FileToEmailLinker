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
            IEnumerable<string>? files = Directory.EnumerateFiles(fullPath, "*.xlsx").Select(file => Path.GetFileName(file));
            return files;
        }

        public async Task<ICollection<AttachmentInfo>> GetAttachments()
        {
            var files = GetFolderFiles();
            ICollection<AttachmentInfo> attachmentInfoList = new List<AttachmentInfo>();
            foreach (var fileName in files)
            {
                AttachmentInfo attachmentInfo = new();
                attachmentInfo.Name = fileName;
                var isAttached = (await mailingPlanService.GetAllMailingPlanListAsync().ToListAsync()).Any(mp => mp.FileStringList.Contains(fileName));
                attachmentInfo.MailingPlanList.AddRange((await mailingPlanService.GetAllMailingPlanListAsync().ToListAsync()).Where(mp => mp.FileStringList.Split(';').Any(fn => fn.Equals(fileName))));
                attachmentInfo.IsDeletable = !isAttached;

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
    }
}

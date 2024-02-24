using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Enums;
using FileToEmailLinker.Models.InputModels.Schedulations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.InputModels.MailPlans
{
    public class MailPlanInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public ActiveState ActiveState { get; set; }
        public bool ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> FilesSelection { get; set; }
        public List<string> ReceiversSelection { get; set; }
        public List<SelectListItem> FileSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ReceiverSelectList { get; set; } = new List<SelectListItem>();
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly? SchedDate { get; set; }
        public TimeOnly SchedTime { get; set; }
        public WeeklyScheduleInputModel? WeeklySchedulation { get; set; }
        public MonthlyScheduleInputModel? MonthlySchedulation { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly EndDate { get; set; } = DateOnly.MaxValue;
    }
}

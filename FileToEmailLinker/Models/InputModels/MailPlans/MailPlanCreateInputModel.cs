﻿using FileToEmailLinker.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileToEmailLinker.Models.InputModels.MailPlans
{
    public class MailPlanCreateInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ActiveState ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> FilesSelection { get; set; }
        public List<string> ReceiversSelection { get; set; }
        public List<SelectListItem> FileSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ReceiverSelectList { get; set; } = new List<SelectListItem>();
        public DateOnly SchedDate { get; set; }
        public TimeOnly SchedTime { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}

using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Enums;
using FileToEmailLinker.Models.InputModels.Schedulations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.InputModels.MailPlans
{
    public class MailPlanCreateInputModel
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
        //public List<bool> WeekDays { get; set; }
        //public string? modello_MondayStr { get; set; }
        //public string? modello_TuesdayStr { get; set; }
        //public string? modello_WednesdayStr { get; set; }
        //public string? modello_ThursdayStr { get; set; }
        //public string? modello_FridayStr { get; set; }
        //public string? modello_SaturdayStr { get; set; }
        //public string? modello_SundayStr { get; set; }
        //public bool Monday { get; set; }
        //public bool Tuesday { get; set; }
        //public bool Wednesday { get; set; }
        //public bool Thursday { get; set; }
        //public bool Friday { get; set; }
        //public bool Saturday { get; set; }
        //public bool Sunday { get; set; }
        //[DisplayName("1")]
        //public bool One { get; set; }
        //[DisplayName("2")]
        //public bool Two { get; set; }
        //[DisplayName("3")]
        //public bool Three { get; set; }
        //[DisplayName("4")]
        //public bool Four { get; set; }
        //[DisplayName("5")]
        //public bool Five { get; set; }
        //[DisplayName("6")]
        //public bool Six { get; set; }
        //[DisplayName("7")]
        //public bool Seven { get; set; }
        //[DisplayName("8")]
        //public bool Eight { get; set; }
        //[DisplayName("9")]
        //public bool Nine { get; set; }
        //[DisplayName("10")]
        //public bool Ten { get; set; }
        //[DisplayName("11")]
        //public bool Eleven { get; set; }
        //[DisplayName("12")]
        //public bool Twelve { get; set; }
        //[DisplayName("13")]
        //public bool Thirteen { get; set; }
        //[DisplayName("14")]
        //public bool Fourteen { get; set; }
        //[DisplayName("15")]
        //public bool Fifteen { get; set; }
        //[DisplayName("16")]
        //public bool Sixteen { get; set; }
        //[DisplayName("17")]
        //public bool Seventeen { get; set; }
        //[DisplayName("18")]
        //public bool Eighteen { get; set; }
        //[DisplayName("19")]
        //public bool Nineteen { get; set; }
        //[DisplayName("20")]
        //public bool Twenty { get; set; }
        //[DisplayName("21")]
        //public bool Twentyone { get; set; }
        //[DisplayName("22")]
        //public bool Twentytwo { get; set; }
        //[DisplayName("23")]
        //public bool Twentythree { get; set; }
        //[DisplayName("24")]
        //public bool Twentyfour { get; set; }
        //[DisplayName("25")]
        //public bool Twentyfive { get; set; }
        //[DisplayName("26")]
        //public bool Twentysix { get; set; }
        //[DisplayName("27")]
        //public bool Twentyseven { get; set; }
        //[DisplayName("28")]
        //public bool Twentyeight { get; set; }
        //[DisplayName("29")]
        //public bool Twentynine { get; set; }
        //[DisplayName("30")]
        //public bool Thirty { get; set; }
        //[DisplayName("31")]
        //public bool Thirtyone { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly EndDate { get; set; } = DateOnly.MaxValue;
    }
}

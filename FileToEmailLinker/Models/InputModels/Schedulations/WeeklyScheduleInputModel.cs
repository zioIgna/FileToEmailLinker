namespace FileToEmailLinker.Models.InputModels.Schedulations
{
    public class WeeklyScheduleInputModel : ISchedulationInputModel
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        //public string? MondayStr { get; set; }
        //public string? TuesdayStr { get; set; }
        //public string? WednesdayStr { get; set; }
        //public string? ThursdayStr { get; set; }
        //public string? FridayStr { get; set; }
        //public string? SaturdayStr { get; set; }
        //public string? SundayStr { get; set; }
    }
}

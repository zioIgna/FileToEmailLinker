﻿using FileToEmailLinker.Models.InputModels.Schedulations;

namespace FileToEmailLinker.Models.Entities
{
    public class WeeklySchedulation : Schedulation
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}

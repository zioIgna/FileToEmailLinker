﻿using FileToEmailLinker.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.Entities
{
    public class Schedulation
    {
        public int Id { get; set; }
        public string Name { get; set; } = DateTime.Now.ToString();
        public SchedulingRecurrence Recurrence { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly EndDate { get; set; } = DateOnly.MaxValue;
        ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}

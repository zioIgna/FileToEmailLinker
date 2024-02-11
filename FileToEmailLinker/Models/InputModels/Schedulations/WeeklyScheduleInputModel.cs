using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.InputModels.Schedulations
{
    public class WeeklyScheduleInputModel : ISchedulationInputModel//,  IValidatableObject
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(!(Monday || Tuesday || Wednesday || Thursday || Friday || Saturday || Sunday))
        //    {
        //        yield return new ValidationResult($"Selezionare almeno un giorno per l'invio settimanale");
        //    }
        //}
    }
}

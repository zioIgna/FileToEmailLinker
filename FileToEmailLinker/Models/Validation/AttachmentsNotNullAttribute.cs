using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.Validation
{
    public class AttachmentsNotNullAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<SelectListItem> myObject = value as List<SelectListItem>;
            if(myObject.Count == 0)
            {
                return new ValidationResult("Selezionare almeno un file da allegare");
            }
            return ValidationResult.Success;
        }
    }
}

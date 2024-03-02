using System.ComponentModel.DataAnnotations;

namespace FileToEmailLinker.Models.InputModels.Receivers
{
    public class ReceiverInputModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}

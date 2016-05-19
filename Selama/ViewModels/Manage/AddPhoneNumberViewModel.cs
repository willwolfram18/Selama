using System.ComponentModel.DataAnnotations;

namespace Selama.ViewModels.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Number { get; set; }
    }
}
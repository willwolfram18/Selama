using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Account.ViewModels.Home
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(25, ErrorMessage = "{0} must be between {1} and {2} characters in length", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
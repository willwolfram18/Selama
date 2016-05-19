using System.ComponentModel.DataAnnotations;

namespace Selama.ViewModels.Manage
{
    public class SetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
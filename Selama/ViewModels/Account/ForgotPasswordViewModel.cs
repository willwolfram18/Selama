using System.ComponentModel.DataAnnotations;

namespace Selama.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Account.ViewModels.Home
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Account.ViewModels.Home
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
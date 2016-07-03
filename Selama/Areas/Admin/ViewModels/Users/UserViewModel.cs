using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            IsEmailConfirmed = user.EmailConfirmed;
            IsActive = user.IsActive;
            WaitingReview = user.WaitingReview;
        }


        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Email Confirmed?")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Enabled")]
        public bool IsActive { get; set; }

        [Display(Name = "Waiting Review?")]
        public bool WaitingReview { get; set; }
    }
}
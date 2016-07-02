using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Account.ViewModels.Manage
{
    public class ApproveUserViewModel
    {
        public ApproveUserViewModel() { }
        public ApproveUserViewModel(ApplicationUser user)
        {
            UserID = user.Id;
            Username = user.UserName;
            Email = user.Email;
        }

        [Required]
        public string UserID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
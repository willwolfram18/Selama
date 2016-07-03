using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(ApplicationUser user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserId = user.Id;
                Username = user.UserName;
                Email = user.Email;
                UserRole = string.Join(", ", db.Users
                                .Where(u => u.Id == user.Id)
                                .SelectMany(u => u.Roles)
                                .Join(db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name));
                IsEmailConfirmed = user.EmailConfirmed;
                IsActive = user.IsActive;
                WaitingReview = user.WaitingReview;
            }
        }


        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "Email Confirmed?")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Premission Role")]
        public string UserRole { get; set; }

        [Display(Name = "Enabled")]
        public bool IsActive { get; set; }

        [Display(Name = "Waiting Review?")]
        public bool WaitingReview { get; set; }
    }
}
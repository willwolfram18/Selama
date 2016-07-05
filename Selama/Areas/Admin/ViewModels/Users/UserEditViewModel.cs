﻿using Microsoft.AspNet.Identity.EntityFramework;
using Selama.Models;
using Selama.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserEditViewModel : _BaseEditableViewModel
    {
        public UserEditViewModel() { }
        public UserEditViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            Username = user.UserName;
            Email = user.Email;
            RoleId = user.Roles.FirstOrDefault().RoleId;
            IsActive = user.IsActive;
            Version = Convert.ToBase64String(user.Version);
        }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Permission Role")]
        public string RoleId { get; set; }

        [Required]
        [Display(Name = "Enabled?")]
        public bool IsActive { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Version { get; set; }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IdentityRole role = db.Roles.Find(RoleId);
                if (role == null)
                {
                    ModelState.AddModelError("RoleId", "Invalid role selected");
                }
            }
            return;
        }
    }
}
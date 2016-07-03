﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserEditViewModel
    {
        [Required]
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
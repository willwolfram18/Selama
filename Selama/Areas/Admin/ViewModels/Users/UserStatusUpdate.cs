using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserStatusUpdateViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}
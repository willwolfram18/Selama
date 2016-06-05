using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadCreateViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Title { get; set; }
        
        [Required]
        [AllowHtml]
        [MinLength(10)]
        public string Content { get; set; }
    }
}
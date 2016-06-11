using MarkdownDeep;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadViewModel : __BaseEditableViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(10)]
        public string Content { get; set; }

        [Display(Name = "Is pinned?")]
        public bool IsPinned { get; set; }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            // Nothing to validate
        }
    }
}
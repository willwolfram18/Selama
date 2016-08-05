using Selama.Areas.Forums.Models;
using Selama.Classes.Utility.Constants;
using Selama.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadEditViewModel : _BaseEditableViewModel
    {
        public ThreadEditViewModel() { }
        public ThreadEditViewModel(Thread thread)
        {
            ID = thread.ID;
            Content = thread.Content;
            Version = Convert.ToBase64String(thread.Version);
        }

        [Required]
        public int ID { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(ForumsConstants.ThreadContentMinLen, ErrorMessage = ForumsConstants.ThreadContentLenErrorMsg)]
        public string Content { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Version { get; set; }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Content.Trim();
            }
        }
    }
}
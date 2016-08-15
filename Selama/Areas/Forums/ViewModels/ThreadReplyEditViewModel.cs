using Selama.Areas.Forums.Models;
using Selama.Common.Utility.Constants;
using Selama.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadReplyEditViewModel : _BaseEditableViewModel
    {
        public ThreadReplyEditViewModel() { }
        public ThreadReplyEditViewModel(ThreadReply reply)
        {
            ID = reply.ID;
            ThreadID = reply.ThreadID;
            Content = reply.Content;
            Version = Convert.ToBase64String(reply.Version);
        }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int ThreadID { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Reply Content")]
        [MinLength(ForumsConstants.ThreadReplyContentMinLen, ErrorMessage = ForumsConstants.ThreadReplyContentLenErrorMsg)]
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
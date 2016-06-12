using Selama.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadReplyViewModel : _BaseEditableViewModel
    {
        [Required]
        public int ThreadID { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Reply Content")]
        [MinLength(50, ErrorMessage = "A reply to a thread must contain at least {1} characters")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime PostDate { get; set; }

        public string Author { get; set; }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            // Nothing to validate
        }
    }
}
using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadReplyViewModel : _BaseEditableViewModel
    {
        public ThreadReplyViewModel() { }
        public ThreadReplyViewModel(ThreadReply reply)
        {
            ID = reply.ID;
            ThreadID = reply.ThreadID;
            Content = reply.Content;
            Markdown markdownTransform = new Markdown()
            {
                SafeMode = true
            };
            HtmlContent = new HtmlString(markdownTransform.Transform(Content));
            PostDate = reply.PostDate;
            Author = reply.Author.UserName;
        }

        public int ID { get; set; }

        [Required]
        public int ThreadID { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Reply Content")]
        [MinLength(50, ErrorMessage = "A reply to a thread must contain at least {1} characters")]
        public string Content { get; set; }

        public HtmlString HtmlContent { get; set; }

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
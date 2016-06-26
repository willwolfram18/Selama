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
        public ThreadReplyViewModel(ThreadReply reply, int replyIndex)
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
            AuthorID = reply.AuthorID;
            Author = reply.Author.UserName;
            // Add 2 because index has a base 0, and want base 1, and the thread content
            // itself is part of the index
            ReplyIndex = replyIndex + 2;
            IsThreadLocked = reply.Thread.IsLocked;
        }

        public int ID { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
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

        public string AuthorID { get; set; }
        public string Author { get; set; }

        public int ReplyIndex { get; set; }

        public bool IsThreadLocked { get; set; }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Content.Trim();
            }
        }
    }
}
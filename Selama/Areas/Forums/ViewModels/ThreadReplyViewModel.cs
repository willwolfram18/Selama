using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.Common.Utility;
using Selama.Common.Utility.Constants;
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
            ID = reply.Id;
            ThreadID = reply.ThreadId;
            Content = reply.Content;

            HtmlContent = new HtmlString(Util.Markdown.Transform(Content));
            PostDate = reply.PostDate;
            AuthorID = reply.AuthorId;
            Author = reply.Author.UserName;
            // Add 1 because index has a base 0, and want base 1, and the thread content
            // itself is part of the index
            ReplyIndex = reply.ReplyIndex + 1;
            IsThreadLocked = reply.Thread.IsLocked;
        }

        public int ID { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int ThreadID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ForumsConstants.ThreadReplyNotEmptyErrorMsg)]
        [AllowHtml]
        [Display(Name = "Reply Content")]
        [MinLength(ForumsConstants.ThreadReplyContentMinLen, ErrorMessage = ForumsConstants.ThreadReplyContentLenErrorMsg)]
        public string Content { get; set; }

        public HtmlString HtmlContent { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
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
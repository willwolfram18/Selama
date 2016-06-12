using MarkdownDeep;
using Selama.Areas.Forums.Models;
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
        public ThreadViewModel() { }
        public ThreadViewModel(Thread thread)
        {
            ID = thread.ID;
            ForumID = thread.ForumID;
            ForumName = thread.Forum.Title;
            Title = thread.Title;

            Content = thread.Content;

            Markdown markdown = new Markdown()
            {
                SafeMode = true,
            };
            HtmlContent = new HtmlString(markdown.Transform(Content));

            IsPinned = thread.IsPinned;
            PostDate = thread.PostDate;
            Author = thread.Author.UserName;
        }

        public int ID { get; set; }

        public int ForumID { get; set; }

        public string ForumName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(10)]
        public string Content { get; set; }

        public HtmlString HtmlContent { get; set; }

        [Display(Name = "Is pinned?")]
        public bool IsPinned { get; set; }

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
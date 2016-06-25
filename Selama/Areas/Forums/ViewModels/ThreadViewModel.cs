using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadViewModel : _BaseEditableViewModel
    {
        public ThreadViewModel() { }
        public ThreadViewModel(Thread thread, int pageSize, int pageNum)
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
            AuthorID = thread.AuthorID;
            Author = thread.Author.UserName;

            PageSize = pageSize;
            PageNum = pageNum;
            if (StartingIndex >= thread.Replies.Count)
            {
                PageNum = thread.Replies.Count / PageSize;
            }

            Replies = new List<ThreadReplyViewModel>();
            var dbReplies = thread.Replies.OrderBy(r => r.PostDate)
                .Skip(StartingIndex)
                .Take(PageSize)
                .ToList();
            int indexOffset = (PageNum == 0 ? 0 : 1);
            for (int i = 0; i < dbReplies.Count; i++)
            {
                ThreadReply reply = dbReplies[i];
                if (reply.IsActive)
                {
                    Replies.Add(new ThreadReplyViewModel(reply, StartingIndex + i));
                }
            }
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

        public string AuthorID { get; set; }
        public string Author { get; set; }

        public List<ThreadReplyViewModel> Replies { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public int StartingIndex
        {
            get
            {
                if (PageNum == 0)
                {
                    return 0;
                }

                return (PageNum * PageSize) - 1;
            }
        }

        public override void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Content.Trim();
            }
            if (!string.IsNullOrEmpty(Title))
            {
                Title = Title.Trim();
            }
        }
    }
}
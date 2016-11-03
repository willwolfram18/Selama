using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.Common.Utility;
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
    public class ThreadViewModel : _BaseEditableViewModel
    {
        public ThreadViewModel() { }
        public ThreadViewModel(Thread thread, int pageSize, int pageNum)
        {
            ID = thread.Id;
            ForumID = thread.ForumId;
            ForumName = thread.Forum.Title;
            Title = thread.Title;

            Content = thread.Content;

            HtmlContent = new HtmlString(Util.Markdown.Transform(Content));

            IsPinned = thread.IsPinned;
            PostDate = thread.PostDate;
            AuthorID = thread.AuthorId;
            Author = thread.Author.UserName;
            IsLocked = thread.IsLocked;

            #region Set pagination variables
            var activeReplies = thread.Replies.Where(t => t.IsActive);
            int pageSizeOffset = (pageNum == 0 ? 1 : 0);
            PageSize = pageSize;
            _pageNum = pageNum;
            // Add 1 for the thread content itself
            NumPages = (int)Math.Ceiling((activeReplies.Count() + 1.0) / (PageSize + pageSizeOffset));
            if (StartingIndex >= activeReplies.Count())
            {
                _pageNum = NumPages - 1;
            }
            #endregion

            #region Get the replies
            Replies = new List<ThreadReplyViewModel>();
            var dbReplies = activeReplies
                .OrderBy(r => r.PostDate)
                .ThenBy(r => r.ReplyIndex)
                .Skip(StartingIndex)
                .Take(PageSize)
                .ToList();
            int indexOffset = (_pageNum == 0 ? 0 : 1);
            foreach (ThreadReply reply in dbReplies)
            {
                Replies.Add(new ThreadReplyViewModel(reply));
            }
            #endregion
        }

        #region Forum/Thread properties
        public int ID { get; set; }

        public int ForumID { get; set; }

        public string ForumName { get; set; }

        [Required]
        [StringLength(ForumsConstants.ThreadTitleMaxLen, MinimumLength = ForumsConstants.ThreadTitleMinLen, ErrorMessage = ForumsConstants.ThreadTitleLenErrorMsg)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(ForumsConstants.ThreadContentMinLen, ErrorMessage = ForumsConstants.ThreadContentLenErrorMsg)]
        public string Content { get; set; }

        public HtmlString HtmlContent { get; set; }

        [Display(Name = "Is pinned?")]
        public bool IsPinned { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime PostDate { get; set; }

        public string AuthorID { get; set; }
        public string Author { get; set; }

        [Display(Name = "Is locked?")]
        public bool IsLocked { get; set; }

        public List<ThreadReplyViewModel> Replies { get; set; }
        #endregion

        #region Pagination properties
        private int _pageNum { get; set; }

        public int ViewPageNum
        {
            get
            {
                return _pageNum + 1;
            }
        }

        public int PageSize { get; set; }

        public int NumPages { get; set; }

        private int StartingIndex
        {
            get
            {
                if (_pageNum == 0)
                {
                    return 0;
                }

                // Subtract 1 to compensate for thread content counting as 1
                return (_pageNum * PageSize) - 1;
            }
        }

        public int PagerStartingIndex
        {
            get
            {
                if (ViewPageNum == 1)
                {
                    return 0;
                }
                else if (ViewPageNum == NumPages)
                {
                    return -2;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        #region Methods
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
        #endregion
    }
}
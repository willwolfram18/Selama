using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.Common.Utility;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        #region Class constants
        private const int MAX_PREVIEW_LEN = 270;
        #endregion

        #region Instance properties
        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        public bool IsLocked { get; set; }

        public bool IsPinned { get; set; }

        public string Preview { get; set; }

        public LastThreadPostViewModel LastPost { get; set; }
        #endregion

        public ThreadOverviewViewModel(Thread thread)
        {
            ID = thread.Id;
            Title = thread.Title;
            NumReplies = thread.GetReplies().Count();
            IsLocked = thread.IsLocked;
            IsPinned = thread.IsPinned;

            InitThreadPreview(thread);
            SetLastPost(thread);
        }

        private void InitThreadPreview(Thread thread)
        {
            // Transform content to HTML, and strip HTML from preview
            Preview = Util.Markdown.Transform(thread.Content);
            Preview = Regex.Replace(Preview, "<.*?>", "");

            int previewLength = (Preview.Length < MAX_PREVIEW_LEN ? Preview.Length : MAX_PREVIEW_LEN);
            Preview = Preview.Substring(0, previewLength);
            if (Preview.Length == MAX_PREVIEW_LEN)
            {
                Preview += "...";
            }
        }

        private void SetLastPost(Thread thread)
        {
            // Set last post to thread initially, overwrite when a reply was added
            LastPost = new LastThreadPostViewModel
            {
                Author = thread.Author.UserName,
                PostDate = thread.PostDate,
            };

            if (thread.Replies.Count > 0)
            {
                ThreadReply lastPost = thread.Replies.OrderByDescending(r => r.PostDate).FirstOrDefault();
                LastPost = new LastThreadPostViewModel
                {
                    Author = lastPost.Author.UserName,
                    PostDate = lastPost.PostDate
                };
            }
        }
    }
}
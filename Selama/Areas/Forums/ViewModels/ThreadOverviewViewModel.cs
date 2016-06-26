using MarkdownDeep;
using Selama.Areas.Forums.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        private int _previewLen = 270;

        public ThreadOverviewViewModel(Thread thread)
        {
            ID = thread.ID;
            Title = thread.Title;
            NumReplies = thread.Replies.Count;
            IsLocked = thread.IsLocked;

            Markdown md = new Markdown
            {
                SafeMode = true,
            };
            Preview = md.Transform(thread.Content);
            Preview = Preview.Substring(0, (Preview.Length < _previewLen ? Preview.Length : _previewLen));
            if (Preview.Length == _previewLen)
            {
                Preview += "...";
            }
            
            if (thread.Replies.Count > 0)
            {

                LastPost = new LastThreadPostViewModel
                {
                    Author = thread.Replies.OrderByDescending(r => r.PostDate).FirstOrDefault().Author.UserName
                };
            }
            else
            {
                LastPost = new LastThreadPostViewModel
                {
                    Author = thread.Author.UserName
                };
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        public bool IsLocked { get; set; }

        public string Preview { get; set; }

        public LastThreadPostViewModel LastPost { get; set; }
    }
}
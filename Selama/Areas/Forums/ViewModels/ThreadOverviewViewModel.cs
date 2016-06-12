using Selama.Areas.Forums.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public ThreadOverviewViewModel(Thread t)
        {
            ID = t.ID;
            Title = t.Title;
            NumReplies = t.Replies.Count;

            ThreadReply tr = t.Replies.OrderByDescending(r => r.PostDate).FirstOrDefault();
            if (tr == null)
            {
                LastPost = new LastThreadPostViewModel
                {
                    Author = tr.Author.UserName
                };
            }
            else
            {
                LastPost = new LastThreadPostViewModel
                {
                    Author = t.Author.UserName
                };
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        public LastThreadPostViewModel LastPost { get; set; }
    }
}
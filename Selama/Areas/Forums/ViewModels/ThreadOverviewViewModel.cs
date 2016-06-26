using Selama.Areas.Forums.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public ThreadOverviewViewModel(Thread thread)
        {
            ID = thread.ID;
            Title = thread.Title;
            NumReplies = thread.Replies.Count;

            
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

        public LastThreadPostViewModel LastPost { get; set; }
    }
}
using Selama.Areas.Forums.Models;
using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public ThreadOverviewViewModel(Thread t)
        {
            ID = t.ID;
            Title = t.Title;
            NumReplies = t.Replies.Count;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        // TODO: Add LastPost/Reply property
    }
}
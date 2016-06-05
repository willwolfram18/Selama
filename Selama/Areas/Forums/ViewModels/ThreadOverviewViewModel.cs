using Selama.Areas.Forums.Models;
using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public ThreadOverviewViewModel(Thread t)
        {

        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        // TODO: Add LastPost/Reply property
    }
}
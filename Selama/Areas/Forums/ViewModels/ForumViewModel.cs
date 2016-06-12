using Selama.Areas.Forums.Models;
using Selama.Classes.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ForumViewModel
    {
        public ForumViewModel(Forum f)
        {
            ID = f.ID;
            Title = f.Title;
            SubTitle = f.SubTitle;

            PinnedThreads = Util.ConvertLists<Thread, ThreadOverviewViewModel>(
                f.PinnedThreads,
                t => new ThreadOverviewViewModel(t)
            );

            Threads = Util.ConvertLists<Thread, ThreadOverviewViewModel>(
                f.Threads.Where(t => t.IsActive),
                t => new ThreadOverviewViewModel(t)
            );
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Subtitle")]
        public string SubTitle { get; set; }

        public IEnumerable<ThreadOverviewViewModel> PinnedThreads { get; set; }

        public IEnumerable<ThreadOverviewViewModel> Threads { get; set; }
    }
}
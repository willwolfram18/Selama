using Selama.Areas.Forums.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ForumSectionViewModel
    {
        public ForumSectionViewModel(ForumSection section)
        {
            Title = section.Title;
            Forums = new List<ForumOverviewViewModel>();
            foreach (Forum f in section.Forums)
            {
                ((List<ForumOverviewViewModel>)Forums).Add(new ForumOverviewViewModel(f));
            }
        }

        public string Title { get; set; }

        public IEnumerable<ForumOverviewViewModel> Forums { get; set; }
    }
}
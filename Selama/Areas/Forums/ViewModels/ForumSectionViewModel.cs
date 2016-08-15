using Selama.Areas.Forums.Models;
using Selama.Common.Utility;
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
            Forums = Util.ConvertLists<Forum, ForumOverviewViewModel>(
                section.Forums.Where(f => f.IsActive),
                f => new ForumOverviewViewModel(f)
            );
        }

        public string Title { get; set; }

        public IEnumerable<ForumOverviewViewModel> Forums { get; set; }
    }
}
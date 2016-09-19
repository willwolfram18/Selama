using Selama.Areas.Forums.Models;
using Selama.Common.ExtensionMethods;
using Selama.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ForumSectionViewModel
    {
        #region Instance properties
        public string Title { get; set; }

        public IEnumerable<ForumOverviewViewModel> Forums { get; set; }
        #endregion

        public ForumSectionViewModel(ForumSection section)
        {
            Title = section.Title;
            Forums = section.Forums.Where(f => f.IsActive).ToListOfDifferentType(
                f => new ForumOverviewViewModel(f)
            );
        }
    }
}
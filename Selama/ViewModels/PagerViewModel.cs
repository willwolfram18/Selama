using Selama.Areas.Admin.ViewModels.Users;
using Selama.Areas.Forums.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.ViewModels
{
    public class PagerViewModel
    {
        public PagerViewModel() { }
        public PagerViewModel(UserOverviewViewModel model)
        {
            BuildPager(model.NumPages, model.ViewPageNum, model.PagerStartingIndex, "Index", "Users", "Admin", null);
        }
        public PagerViewModel(ForumViewModel model)
        {
            BuildPager(model.NumPages, model.ViewPageNum, model.PagerStartingIndex, "Threads", "Forum", "Forums", model.ID);
        }
        public PagerViewModel(ThreadViewModel model)
        {
            BuildPager(model.NumPages, model.ViewPageNum, model.PagerStartingIndex, "Thread", "Forum", "Forums", model.ID);
        }

        private void BuildPager(int numPages, int currentPage, int btnStartingIndex, string action,
            string controller, string area, object id)
        {
            NumPages = numPages;
            CurrentPage = currentPage;
            BtnStartingIndex = btnStartingIndex;
            Action = action;
            Controller = controller;
            Area = area;
            Id = id;
        }

        public int NumPages { get; set; }

        public int CurrentPage { get; set; }

        public int BtnStartingIndex { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public string Area { get; set; }

        public object Id { get; set; }
    }
}
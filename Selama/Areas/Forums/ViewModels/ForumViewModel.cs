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
        public ForumViewModel(Forum forum, int pageSize, int pageNum)
        {
            ID = forum.ID;
            Title = forum.Title;
            SubTitle = forum.SubTitle;

            #region Set paging properties
            var activeThreads = forum.Threads.Where(t => t.IsActive);
            _pageNum = pageNum;
            _pageSize = pageSize;
            NumPages = (int)Math.Ceiling((1.0 * activeThreads.Count()) / _pageSize);
            if (StartingIndex >= activeThreads.Count())
            {
                _pageNum = NumPages - 1;
            }
            #endregion

            #region Get threads
            Threads = Util.ConvertLists<Thread, ThreadOverviewViewModel>(
                activeThreads.OrderByDescending(t => t.IsPinned)
                    .ThenByDescending(t => t.PostDate)
                    .Skip(StartingIndex)
                    .Take(_pageSize),
                t => new ThreadOverviewViewModel(t)
            );
            #endregion
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Subtitle")]
        public string SubTitle { get; set; }

        public IEnumerable<ThreadOverviewViewModel> Threads { get; set; }

        #region Pagination properties
        private int _pageSize { get; set; }

        private int _pageNum { get; set; }

        private int StartingIndex
        {
            get
            {
                if (_pageNum == 0)
                {
                    return 0;
                }

                return (_pageSize * _pageNum);
            }
        }

        public int ViewPageNum
        {
            get
            {
                return _pageNum + 1;
            }
        }

        public int PagerStartingIndex
        {
            get
            {
                if (ViewPageNum == 1)
                {
                    return 0;
                }
                else if (ViewPageNum == NumPages)
                {
                    return -2;
                }
                else
                {
                    return -1;
                }
            }
        }

        public int NumPages { get; set; }
        #endregion
    }
}
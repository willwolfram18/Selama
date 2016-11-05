using Selama.Areas.Forums.Models;
using Selama.Common.Extensions;
using Selama.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ForumViewModel
    {
        #region Instance properties
        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Subtitle")]
        public string SubTitle { get; set; }

        public IEnumerable<ThreadOverviewViewModel> Threads { get; set; }

        #region Pagination properties
        private int _pageSize { get; set; }

        private int _currentPage { get; set; }

        private int StartingIndex
        {
            get
            {
                if (_currentPage == 0)
                {
                    return 0;
                }

                return (_pageSize * _currentPage);
            }
        }

        public int ViewPageNum
        {
            get
            {
                return _currentPage + 1;
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
        #endregion

        public ForumViewModel(Forum forum, int pageSize, int currentPage)
        {
            ID = forum.Id;
            Title = forum.Title;
            SubTitle = forum.SubTitle;

            var activeThreads = forum.Threads.Where(t => t.IsActive);

            _pageSize = pageSize;
            _currentPage = currentPage;
            SetNumPagesAndCurrentPage(forum, activeThreads.Count());

            #region Get threads
            Threads = activeThreads.Skip(StartingIndex).Take(_pageSize).ToListOfDifferentType(
                t => new ThreadOverviewViewModel(t)
            ).OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.LastPost.PostDate);
            #endregion
        }

        private void SetNumPagesAndCurrentPage(Forum forum, int numActiveThreads)
        {
            NumPages = Math.Max(CalculateNumPages(numActiveThreads), 1);

            if (StartingIndex >= numActiveThreads)
            {
                _currentPage = NumPages - 1;
            }
        }

        private int CalculateNumPages(int numActiveThreads)
        {
            return (int)Math.Ceiling((1.0 * numActiveThreads) / _pageSize);
        }
    }
}
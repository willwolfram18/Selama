using Selama.Common.Extensions;
using Selama.Common.Utility;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Selama.Areas.Admin.ViewModels.Users
{
    public class UserOverviewViewModel
    {
        public UserOverviewViewModel()
        {
            Users = new List<UserViewModel>();
        }
        public UserOverviewViewModel(List<ApplicationUser> users, int pageSize, int pageNum)
        {
            #region Pagination variables
            PageSize = pageSize;
            _pageNum = pageNum;
            NumPages = (int)Math.Ceiling((users.Count * 1.0) / PageSize);
            if (StartingIndex >= users.Count)
            {
                _pageNum = NumPages - 1;
            }
            #endregion

            Users = users.Skip(StartingIndex).Take(pageSize).ToListOfDifferentType(
                u => new UserViewModel(u)
            );
        }

        public List<UserViewModel> Users { get; set; }

        private int _pageNum { get; set; }

        public int ViewPageNum
        {
            get
            {
                return _pageNum + 1;
            }
        }

        public int PageSize { get; set; }

        public int NumPages { get; set; }

        private int StartingIndex
        {
            get
            {
                if (_pageNum == 0)
                {
                    return 0;
                }

                return (_pageNum * PageSize);
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
    }
}
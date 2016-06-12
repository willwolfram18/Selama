using Selama.Areas.Forums.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ForumOverviewViewModel
    {
        public ForumOverviewViewModel(Forum f)
        {
            ID = f.ID;
            Title = f.Title;
            SubTitle = f.SubTitle;
            NumThreads = f.Threads.Count();

            Thread lastPost = f.Threads.OrderByDescending(t => t.Replies.Select(r => r.PostDate)).FirstOrDefault();
            if (lastPost != null)
            {
                LastPost = new LastForumPostViewModel
                {
                    ThreadID = lastPost.ID,
                    ThreadTitle = lastPost.Title,
                    Author = lastPost.Author.UserName,
                };
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Subtitle")]
        public string SubTitle { get; set; }

        [Display(Name = "Threads")]
        public int NumThreads { get; set; }

        public LastForumPostViewModel LastPost { get; set; }
        // TODO: Add LastPost attribute
    }
}
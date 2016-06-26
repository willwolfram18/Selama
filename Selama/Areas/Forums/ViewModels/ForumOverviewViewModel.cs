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
        public ForumOverviewViewModel(Forum forum)
        {
            ID = forum.ID;
            Title = forum.Title;
            SubTitle = forum.SubTitle;
            NumThreads = forum.Threads.Count();

            Thread lastPost = null;
            DateTime? lastPostDate = null;
            foreach (Thread thread in forum.Threads.Where(t => t.IsActive))
            {
                DateTime threadPostDate = thread.PostDate;
                if (thread.Replies.Count > 0)
                {
                    // if a thread has a reply, it has to follow its creation therefore guaranteed more new
                    threadPostDate = thread.Replies.Where(t => t.IsActive).OrderByDescending(r => r.PostDate).FirstOrDefault().PostDate;
                }

                if (lastPost == null || lastPostDate.Value < threadPostDate)
                {
                    lastPost = thread;
                    lastPostDate = threadPostDate;
                }
            }
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
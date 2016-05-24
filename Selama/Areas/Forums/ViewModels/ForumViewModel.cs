using Selama.Areas.Forums.Models;
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
            NumThreads = f.Threads.Count();
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Subtitle")]
        public string SubTitle { get; set; }

        [Display(Name = "Threads")]
        public int NumThreads { get; set; }

        // TODO: Add LastPost attribute
    }
}
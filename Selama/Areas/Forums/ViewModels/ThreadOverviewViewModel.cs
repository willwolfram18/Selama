﻿using MarkdownDeep;
using Selama.Areas.Forums.Models;
using Selama.Classes.Utility;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        private int _previewLen = 270;

        public ThreadOverviewViewModel(Thread thread)
        {
            ID = thread.ID;
            Title = thread.Title;
            NumReplies = thread.Replies.Where(r => r.IsActive).Count();
            IsLocked = thread.IsLocked;

            Preview = Util.Markdown.Transform(thread.Content);
            Preview = Regex.Replace(Preview, "<.*?>", "");
            Preview = Preview.Substring(0, (Preview.Length < _previewLen ? Preview.Length : _previewLen));
            if (Preview.Length == _previewLen)
            {
                Preview += "...";
            }
            
            if (thread.Replies.Count > 0)
            {
                LastPost = new LastThreadPostViewModel
                {
                    Author = thread.Replies.OrderByDescending(r => r.PostDate).FirstOrDefault().Author.UserName
                };
            }
            else
            {
                LastPost = new LastThreadPostViewModel
                {
                    Author = thread.Author.UserName
                };
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Replies")]
        public int NumReplies { get; set; }

        public bool IsLocked { get; set; }

        public string Preview { get; set; }

        public LastThreadPostViewModel LastPost { get; set; }
    }
}
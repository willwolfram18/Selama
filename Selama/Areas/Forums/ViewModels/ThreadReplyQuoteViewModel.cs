using Selama.Areas.Forums.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class ThreadReplyQuoteViewModel
    {
        public ThreadReplyQuoteViewModel() { }
        public ThreadReplyQuoteViewModel(Thread thread)
        {
            Author = thread.Author.UserName;
            PostDate = thread.PostDate;
            ReplyIndex = 1;
            PageNum = 1;
            Content = thread.Content;
        }
        public ThreadReplyQuoteViewModel(ThreadReply reply, int page)
        {
            Author = reply.Author.UserName;
            PostDate = reply.PostDate;
            ReplyIndex = reply.ReplyIndex + 1;
            PageNum = page;
            Content = reply.Content;
        }

        public string Author { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime PostDate { get; set; }

        public int ReplyIndex { get; set; }

        public int PageNum { get; set; }
        
        public string Content { get; set; }
    }
}
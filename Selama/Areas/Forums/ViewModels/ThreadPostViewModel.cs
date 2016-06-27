using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public enum PostType
    {
        Thread,
        ThreadReply
    }

    public class ThreadPostViewModel
    {
        public ThreadPostViewModel(ThreadViewModel thread)
        {
            ID = thread.ID;
            PostType = PostType.Thread;
            PostDate = thread.PostDate;
            AuthorID = thread.AuthorID;
            Author = thread.Author;
            HtmlContent = thread.HtmlContent;
            IsLocked = thread.IsLocked;
            PostIndex = 1;
        }
        
        public ThreadPostViewModel(ThreadReplyViewModel reply)
        {
            ID = reply.ID;
            PostType = PostType.ThreadReply;
            PostDate = reply.PostDate;
            AuthorID = reply.AuthorID;
            Author = reply.Author;
            HtmlContent = reply.HtmlContent;
            IsLocked = reply.IsThreadLocked;
            PostIndex = reply.ReplyIndex;
        }

        public int ID { get; set; }

        public PostType PostType { get; set; }

        public DateTime PostDate { get; set; }

        public string AuthorID { get; set; }

        public string Author { get; set; }

        public bool IsLocked { get; set; }

        public int PostIndex { get; set; }

        public HtmlString HtmlContent { get; set; }
    }
}
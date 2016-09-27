using Selama.Areas.Forums.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Selama.Models
{
    [Table("GuildNewsFeed")]
    public class GuildNewsFeedItem
    {
        #region Database columns
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Content { get; set; }
        #endregion

        public GuildNewsFeedItem()
        {
        }

        public GuildNewsFeedItem(Thread thread, string threadUrl)
        {
            Timestamp = DateTime.Now;
            Content = string.Format("{0} posted <a href='{1}'>{2}</a>.", thread.Author.UserName, threadUrl, thread.Title);
        }
    }
}
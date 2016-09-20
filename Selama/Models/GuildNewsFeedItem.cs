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
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
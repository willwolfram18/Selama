using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Models
{
    public class Thread
    {
        public Thread() { }
        public Thread(ThreadViewModel model, string authorId, int forumId)
        {
            Title = model.Title;
            Content = model.Content;
            PostDate = model.PostDate;
            AuthorID = authorId;
            ForumID = forumId;
            IsActive = true;
            IsPinned = model.IsPinned;
        }

        #region Database columns
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "The title's length must be between {0} and {1} characters", MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(50, ErrorMessage = "A thread's content must contain at least {0} characters")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Posted On")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime PostDate { get; set; }

        [Required]
        [ForeignKey("Author")]
        public string AuthorID { get; set; }

        [Required]
        [ForeignKey("Forum")]
        public int ForumID { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsPinned { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation properties
        [InverseProperty("Threads")]
        public virtual Forum Forum { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual ICollection<ThreadReply> Replies { get; set; }
        #endregion
    }
}
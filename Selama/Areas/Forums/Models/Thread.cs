using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Common.Utility.Constants;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Models
{
    public class Thread
    {
        #region Database columns
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(ForumsConstants.ThreadTitleMaxLen, ErrorMessage = ForumsConstants.ThreadTitleLenErrorMsg, MinimumLength = ForumsConstants.ThreadTitleMinLen)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(ForumsConstants.ThreadContentMinLen, ErrorMessage = ForumsConstants.ThreadContentLenErrorMsg)]
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

        [Required]
        public bool IsLocked { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation properties
        [InverseProperty("Threads")]
        public virtual Forum Forum { get; set; }
        
        public virtual ApplicationUser Author { get; set; }
        
        public virtual ICollection<ThreadReply> Replies { get; set; }
        #endregion

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
            IsLocked = model.IsLocked;
        }

        public IEnumerable<ThreadReply> GetReplies()
        {
            return Replies.Where(r => r.IsActive);
        }

        public void UpdateFromViewModel(ThreadEditViewModel viewModel)
        {
            Content = viewModel.Content;
            Convert.FromBase64String(viewModel.Version).CopyTo(Version, 0);
        }

        public static bool CanPinOrLockThreads(IPrincipal User)
        {
            return User.IsInRole("Admin") || User.IsInRole("Forum Mod");
        }
    }
}
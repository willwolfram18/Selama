using Selama.Areas.Forums.ViewModels;
using Selama.Common.Utility.Constants;
using Selama.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Models
{
    public class ThreadReply
    {
        #region Database columns
        [Key]
        public int Id { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(ForumsConstants.ThreadReplyContentMinLen, ErrorMessage = ForumsConstants.ThreadReplyContentLenErrorMsg)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Posted On")]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime PostDate { get; set; }

        [Required]
        [ForeignKey("Author")]
        [InverseProperty("ThreadReplies")]
        public string AuthorId { get; set; }

        [Required]
        [ForeignKey("Thread")]
        public int ThreadId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int ReplyIndex { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation properties
        public virtual Thread Thread { get; set; }
        public virtual ApplicationUser Author { get; set; }
        #endregion

        public ThreadReply() { }
        public ThreadReply(ThreadReplyViewModel model, string authorId, Thread thread)
        {
            Content = model.Content;
            PostDate = DateTime.Now;
            ThreadId = thread.Id;
            Thread = thread;
            AuthorId = authorId;
            ReplyIndex = thread.Replies.Count + 1;
            IsActive = true;
        }

        public void UpdateFromViewModel(ThreadReplyEditViewModel viewModel)
        {
            Content = viewModel.Content;
            Version = Convert.FromBase64String(viewModel.Version);
        }
    }
}
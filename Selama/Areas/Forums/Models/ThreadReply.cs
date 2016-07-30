using Selama.Areas.Forums.ViewModels;
using Selama.Classes.Utility.Constants;
using Selama.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Models
{
    public class ThreadReply
    {
        public ThreadReply() { }
        public ThreadReply(ThreadReplyViewModel model, string authorId, int threadId, int replyIndex)
        {
            Content = model.Content;
            PostDate = model.PostDate;
            ThreadID = threadId;
            AuthorID = authorId;
            ReplyIndex = replyIndex;
            IsActive = true;
        }

        public void UpdateFromViewModel(ThreadReplyEditViewModel viewModel)
        {
            Content = viewModel.Content;
            Version = Convert.FromBase64String(viewModel.Version);
        }

        #region Database columns
        [Key]
        public int ID { get; set; }

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
        public string AuthorID { get; set; }

        [Required]
        [ForeignKey("Thread")]
        public int ThreadID { get; set; }

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
    }
}
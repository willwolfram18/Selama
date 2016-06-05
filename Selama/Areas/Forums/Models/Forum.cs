using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Selama.Areas.Forums.Models
{
    [Table("Forums")]
    public class Forum
    {
        #region Database columns
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A title is required")]
        [StringLength(30, ErrorMessage = "The title's length must be between {0} and {1} characters", MinimumLength = 4)]
        public string Title { get; set; }

        [StringLength(85, ErrorMessage = "The subtitle's length cannot exceed {0}")]
        public string SubTitle { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("ForumSection")]
        public int ForumSectionID { get; set; }
        #endregion

        #region Navigation properties
        public virtual ForumSection ForumSection { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        #endregion

        [NotMapped]
        public virtual ICollection<Thread> PinnedThreads
        {
            get
            {
                if (Threads == null)
                {
                    return null;
                }
                return Threads.Where(t => t.IsPinned && t.IsActive).ToList();
            }
        }
    }
}
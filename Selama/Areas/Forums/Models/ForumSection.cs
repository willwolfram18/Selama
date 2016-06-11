using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Forums.Models
{
    public class ForumSection
    {
        #region Database Columns
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A title must be provided")]
        [StringLength(35, ErrorMessage = "The title's length must be between {0} and {1} characters long", MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation properties
        public virtual ICollection<Forum> Forums { get; set; }
        #endregion
    }
}
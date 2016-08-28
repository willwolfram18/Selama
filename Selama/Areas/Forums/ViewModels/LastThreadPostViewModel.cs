using Selama.Common.ExtensionMethods;
using Selama.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class LastThreadPostViewModel
    {
        public string Author { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime PostDate { get; set; }

        // For display purposes, while PostDate will be the tooltip
        public string DisplayDate
        {
            get
            {
                return PostDate.ToRelativeDateTimeString();
            }
        }
    }
}
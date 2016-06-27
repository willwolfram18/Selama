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

        public DateTime PostDate { get; set; }

        public string DisplayDate
        {
            get
            {
                return PostDate.ToString("f");
            }
        }
    }
}
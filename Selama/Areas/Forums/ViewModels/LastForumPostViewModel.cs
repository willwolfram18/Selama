using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.ViewModels
{
    public class LastForumPostViewModel
    {
        public int ThreadID { get; set; }

        public string ThreadTitle { get; set; }

        public string Author { get; set; }
    }
}
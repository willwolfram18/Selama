using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL
{
    public class ForumRepository : GenericEntityRepository<ApplicationDbContext, Forum>
    {
        public ForumRepository(ApplicationDbContext context) : base(context) { }
    }
}
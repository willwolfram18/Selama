using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL.Repositories
{
    public class ThreadReplyRepository : GenericEntityRepository<ApplicationDbContext, ThreadReply>
    {
        public ThreadReplyRepository(ApplicationDbContext context) : base(context) { }
    }
}
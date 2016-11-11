using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Models.DAL
{
    public class GuildNewsFeedRepository : EntityRepositoryBase<ApplicationDbContext, GuildNewsFeedItem>
    {
        public GuildNewsFeedRepository(ApplicationDbContext context) : base(context) { }
    }
}
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Areas.Forums.Models.DAL
{
    public interface IForumsUnitOfWork : IGenericUnitOfWork
    {
        IGenericEntityRepository<Forum> Forums { get; }
        IGenericEntityRepository<ForumSection> ForumSections { get; }
        IGenericEntityRepository<Thread> Threads { get; }
        IGenericEntityRepository<ThreadReply> ThreadReplies { get; }
        IGenericEntityRepository<ApplicationUser> Authors { get; }
        IGenericEntityRepository<GuildNewsFeedItem> GuildNewsFeedItems { get; }

        Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId);
        void DeleteThread(Thread thread);
        Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl);

    }
}

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
    public interface IForumsUnitOfWork : IUnitOfWork
    {
        IEntityRepository<Forum> Forums { get; }
        IEntityRepository<ForumSection> ForumSections { get; }
        IEntityRepository<Thread> Threads { get; }
        IEntityRepository<ThreadReply> ThreadReplies { get; }
        IEntityRepository<ApplicationUser> Authors { get; }
        IEntityRepository<GuildNewsFeedItem> GuildNewsFeedItems { get; }

        Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId);
        void DeleteThread(Thread thread);
        Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl);

    }
}

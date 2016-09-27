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
        IGenericEntityRepository<Forum> ForumRepository { get; }
        IGenericEntityRepository<ForumSection> ForumSectionRepository { get; }
        IGenericEntityRepository<Thread> ThreadRepository { get; }
        IGenericEntityRepository<ThreadReply> ThreadReplyRepository { get; }
        IGenericEntityRepository<ApplicationUser> IdentityRepository { get; }

        Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId);
        void DeleteThread(Thread thread);
        Task AddNewThreadToNewsFeed(Thread thread, string threadUrl);

    }
}

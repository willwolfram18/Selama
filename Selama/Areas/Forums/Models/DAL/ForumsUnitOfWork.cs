using Microsoft.AspNet.Identity;
using Selama.Areas.Forums.Models.DAL.Repositories;
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL
{
    public class ForumsUnitOfWork : UnitOfWorkBase<ApplicationDbContext>, IForumsUnitOfWork
    {
        public IEntityRepository<Forum> Forums { get; private set; }
        public IEntityRepository<ForumSection> ForumSections { get; private set; }
        public IEntityRepository<Thread> Threads { get; private set; }
        public IEntityRepository<ThreadReply> ThreadReplies { get; private set; }
        public IEntityRepository<ApplicationUser> Authors { get; private set; }
        public IEntityRepository<GuildNewsFeedItem> GuildNewsFeedItems { get; private set; }

        public ForumsUnitOfWork()
        {
            Context = new ApplicationDbContext();
            Forums = new ForumRepository(Context);
            ForumSections = new ForumSectionRepository(Context);
            Threads = new ThreadRepository(Context);
            ThreadReplies = new ThreadReplyRepository(Context);
            Authors = new IdentityRepository(Context);
            GuildNewsFeedItems = new GuildNewsFeedRepository(Context);
        }

        public Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId)
        {
            Thread dbThread = new Thread(threadToCreate, author.Identity.GetUserId(), forumId);
            dbThread.PostDate = DateTime.Now;
            if (!Thread.CanPinOrLockThreads(author))
            {
                dbThread.IsPinned = dbThread.IsLocked = false;
            }
            Threads.Add(dbThread);
            return dbThread;
        }

        public void DeleteThread(Thread thread)
        {
            thread.IsActive = false;
            foreach (var reply in thread.Replies)
            {
                reply.IsActive = false;
                ThreadReplies.Update(reply);
            }
            Threads.Update(thread);
        }

        public async Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl)
        {
            Context.Entry(thread).Reference(t => t.Author).Load();
            GuildNewsFeedItem news = new GuildNewsFeedItem(thread, threadUrl);
            GuildNewsFeedItems.Add(news);
            await SaveChangesAsync();
        }
    }
}
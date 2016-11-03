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
    public class ForumsUnitOfWork : GenericUnitOfWork<ApplicationDbContext>, IForumsUnitOfWork
    {
        public IGenericEntityRepository<Forum> Forums { get; private set; }
        public IGenericEntityRepository<ForumSection> ForumSections { get; private set; }
        public IGenericEntityRepository<Thread> Threads { get; private set; }
        public IGenericEntityRepository<ThreadReply> ThreadReplies { get; private set; }
        public IGenericEntityRepository<ApplicationUser> Authors { get; private set; }
        public IGenericEntityRepository<GuildNewsFeedItem> GuildNewsFeedItems { get; private set; }

        public ForumsUnitOfWork()
        {
            _context = new ApplicationDbContext();
            Forums = new ForumRepository(_context);
            ForumSections = new ForumSectionRepository(_context);
            Threads = new ThreadRepository(_context);
            ThreadReplies = new ThreadReplyRepository(_context);
            Authors = new IdentityRepository(_context);
            GuildNewsFeedItems = new GuildNewsFeedRepository(_context);
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
            _context.Entry(thread).Reference(t => t.Author).Load();
            GuildNewsFeedItem news = new GuildNewsFeedItem(thread, threadUrl);
            GuildNewsFeedItems.Add(news);
            await SaveChangesAsync();
        }
    }
}
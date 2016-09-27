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
        public IGenericEntityRepository<Forum> ForumRepository { get; private set; }
        public IGenericEntityRepository<ForumSection> ForumSectionRepository { get; private set; }
        public IGenericEntityRepository<Thread> ThreadRepository { get; private set; }
        public IGenericEntityRepository<ThreadReply> ThreadReplyRepository { get; private set; }
        public IGenericEntityRepository<ApplicationUser> IdentityRepository { get; private set; }
        public IGenericEntityRepository<GuildNewsFeedItem> GuildNewsFeedRepository { get; private set; }

        public ForumsUnitOfWork()
        {
            _context = new ApplicationDbContext();
            ForumRepository = new ForumRepository(_context);
            ForumSectionRepository = new ForumSectionRepository(_context);
            ThreadRepository = new ThreadRepository(_context);
            ThreadReplyRepository = new ThreadReplyRepository(_context);
            IdentityRepository = new IdentityRepository(_context);
            GuildNewsFeedRepository = new GuildNewsFeedRepository(_context);
        }

        public Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId)
        {
            Thread dbThread = new Thread(threadToCreate, author.Identity.GetUserId(), forumId);
            dbThread.PostDate = DateTime.Now;
            if (!Thread.CanPinOrLockThreads(author))
            {
                dbThread.IsPinned = dbThread.IsLocked = false;
            }
            ThreadRepository.Add(dbThread);
            return dbThread;
        }

        public void DeleteThread(Thread thread)
        {
            thread.IsActive = false;
            foreach (var reply in thread.Replies)
            {
                reply.IsActive = false;
                ThreadReplyRepository.Update(reply);
            }
            ThreadRepository.Update(thread);
        }

        public async Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl)
        {
            _context.Entry(thread).Reference(t => t.Author).Load();
            GuildNewsFeedItem news = new GuildNewsFeedItem(thread, threadUrl);
            GuildNewsFeedRepository.Add(news);
            await SaveChangesAsync();
        }
    }
}
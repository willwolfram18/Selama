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
    public class ForumsUnitOfWork : GenericUnitOfWork<ApplicationDbContext>
    {
        public ForumRepository ForumRepository { get; private set; }
        public ForumSectionRepository ForumSectionRepository { get; private set; }
        public ThreadRepository ThreadRepository { get; private set; }
        public ThreadReplyRepository ThreadReplyRepository { get; private set; }
        public IdentityRepository IdentityRepository { get; private set; }
        private GuildNewsFeedRepository GuildNewsFeedRepository { get; set; }

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

        public async Task AddNewThreadToNewsFeed(Thread thread, string threadUrl)
        {
            _context.Entry(thread).Reference(t => t.Author).Load();
            GuildNewsFeedItem news = new GuildNewsFeedItem
            {
                Timestamp = thread.PostDate,
                Content = string.Format("{0} posted <a href='{1}'>{2}</a>.", thread.Author.UserName,
                    threadUrl, thread.Title),
            };
            GuildNewsFeedRepository.Add(news);
            await SaveChangesAsync();
        }
    }
}
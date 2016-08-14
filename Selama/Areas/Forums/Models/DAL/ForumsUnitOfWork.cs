using Selama.Areas.Forums.Models.DAL.Repositories;
using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ForumsUnitOfWork()
        {
            _context = new ApplicationDbContext();
            ForumRepository = new ForumRepository(_context);
            ForumSectionRepository = new ForumSectionRepository(_context);
            ThreadRepository = new ThreadRepository(_context);
            ThreadReplyRepository = new ThreadReplyRepository(_context);
            IdentityRepository = new IdentityRepository(_context);
        }
    }
}
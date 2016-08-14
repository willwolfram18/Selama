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
        private ForumRepository _forumRepo;
        private ThreadRepository _threadRepo;

        public ForumsUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _forumRepo = new ForumRepository(_context);
            _threadRepo = new ThreadRepository(_context);
        }
    }
}
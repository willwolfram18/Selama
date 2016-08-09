using Selama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL
{
    public class UnitOfWork
    {
        private ApplicationDbContext _context;
        private ForumRepository _forumRepo;
        private ThreadRepository _threadRepo;

        public ForumRepository ForumRepository
        {
            get
            {
                if (_forumRepo == null)
                {
                    _forumRepo = new ForumRepository(_context);
                }
                return _forumRepo;
            }
        }
        public ThreadRepository ThredRepository
        {
            get
            {
                if (_threadRepo == null)
                {
                    _threadRepo = new ThreadRepository(_context);
                }
                return _threadRepo;
            }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
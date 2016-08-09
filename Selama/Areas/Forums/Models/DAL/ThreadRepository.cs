using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL
{
    public class ThreadRepository : IEntityRepository<Thread>, IDisposable
    {
        private ApplicationDbContext _context;

        public ThreadRepository(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _context = context;
        }

        public void Add(Thread entity)
        {
            _context.Threads.Add(entity);
        }

        public void Delete(Thread entity)
        {
            _context.Threads.Remove(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Thread Find(int id)
        {
            return _context.Threads.Find(id);
        }

        public IQueryable<Thread> GetEntities()
        {
            return _context.Threads.AsQueryable();
        }

        public void Update(Thread entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
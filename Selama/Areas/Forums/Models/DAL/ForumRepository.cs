using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Areas.Forums.Models.DAL
{
    public class ForumRepository : IEntityRepository<Forum>, IDisposable
    {
        private ApplicationDbContext _context;

        public ForumRepository(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _context = context;
        }

        public void Add(Forum entity)
        {
            _context.Forums.Add(entity);
        }

        public void Delete(Forum entity)
        {
            _context.Forums.Remove(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Forum Find(int id)
        {
            return _context.Forums.Find(id);
        }

        public IQueryable<Forum> GetEntities()
        {
            return _context.Forums.AsQueryable();
        }

        public void Update(Forum entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
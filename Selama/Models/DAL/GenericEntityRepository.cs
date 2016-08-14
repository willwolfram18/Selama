using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Models.DAL
{
    public class GenericEntityRepository<TContext, TEntity> : IDisposable
        where TContext : DbContext
        where TEntity : class
    {
        private TContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericEntityRepository(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }

            return query;
        }

        public TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }
        public async Task<TEntity> FindByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        
        public void RemoveById(object id)
        {
            TEntity entityToRemove = _dbSet.Find(id);
            _dbSet.Remove(entityToRemove);
        }
        public async Task RemoveByIdAsync(object id)
        {
            TEntity entityToRemove = await _dbSet.FindAsync(id);
            _dbSet.Remove(entityToRemove);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
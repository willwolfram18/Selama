using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Models.DAL
{
    public abstract class EntityRepositoryBase<TContext, TEntity> : IEntityRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        private TContext Context { get; set; }
        private DbSet<TEntity> DbSet { get; set; }

        public EntityRepositoryBase(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            IQueryable<TEntity> query = DbSet;

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
            return DbSet.Find(id);
        }
        public async Task<TEntity> FindByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public void RemoveById(object id)
        {
            TEntity entityToRemove = DbSet.Find(id);
            DbSet.Remove(entityToRemove);
        }
        public async Task RemoveByIdAsync(object id)
        {
            TEntity entityToRemove = await DbSet.FindAsync(id);
            DbSet.Remove(entityToRemove);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
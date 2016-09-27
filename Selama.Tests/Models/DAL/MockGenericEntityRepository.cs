using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Selama.Models;

namespace Selama.Tests.Models.DAL
{
    public class MockGenericEntityRepository<TEntity> : IGenericEntityRepository<TEntity>
        where TEntity : class
    {
        private int _nextId = 1;
        private Dictionary<object, TEntity> _source = new Dictionary<object, TEntity>();

        public void Add(TEntity entity)
        {
            var entityId = _nextId++;
            _source[entityId] = entity;
        }

        public TEntity FindById(object id)
        {
            if (!_source.ContainsKey(id))
            {
                return null;
            }
            return _source[id];
        }

        public async Task<TEntity> FindByIdAsync(object id)
        {
            return FindById(id);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> result = _source.Values.AsQueryable();
            if (filter != null)
            {
                result = result.Where(filter);
            }
            if (orderBy != null)
            {
                result = orderBy(result).AsQueryable();
            }
            return result;
        }

        public void Remove(TEntity entity)
        {
            object entityId = GetObjectId(entity);
            _source.Remove(entityId);
        }

        public void RemoveById(object id)
        {
            _source.Remove(id);
        }

        public Task RemoveByIdAsync(object id)
        {
            RemoveById(id);
            return Task.FromResult(0);
        }

        public void Update(TEntity entity)
        {
            var objectId = GetObjectId(entity);
            _source[objectId] = entity;
        }

        public void Dispose()
        {
        }

        private object GetObjectId(TEntity entity)
        {
            return entity.GetType().GetProperty("ID").GetValue(entity);
        }
    }
}

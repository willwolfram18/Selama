using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Models.DAL
{
    public interface IGenericEntityRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        TEntity FindById(object id);
        Task<TEntity> FindByIdAsync(object id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
        void RemoveById(object id);
        Task RemoveByIdAsync(object id);
    }
}

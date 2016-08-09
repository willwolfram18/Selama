using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Models.DAL
{
    public interface IEntityRepository<TEntity>
    {
        IQueryable<TEntity> GetEntities();
        TEntity Find(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
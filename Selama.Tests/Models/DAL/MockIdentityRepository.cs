using Selama.Models;
using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Selama.Tests.Models.DAL
{
    public class MockIdentityRepository : IGenericEntityRepository<ApplicationUser>
    {
        private Dictionary<string, ApplicationUser> _source = new Dictionary<string, ApplicationUser>();

        public void Add(ApplicationUser entity)
        {
            var entityId = new Guid().ToString();
            _source[entityId] = entity;
            entity.Id = entityId;
        }

        public void Dispose()
        {
        }

        public ApplicationUser FindById(object id)
        {
            if (!_source.ContainsKey(id.ToString()))
            {
                return null;
            }
            return _source[id.ToString()];
        }

        public async Task<ApplicationUser> FindByIdAsync(object id)
        {
            return FindById(id);
        }

        public IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            var result = _source.Values.AsQueryable();
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

        public void Remove(ApplicationUser entity)
        {
            RemoveById(entity.Id);
        }

        public void RemoveById(object id)
        {
            if (_source.ContainsKey(id.ToString()))
            {
                _source.Remove(id.ToString());
            }
        }

        public async Task RemoveByIdAsync(object id)
        {
            RemoveById(id);
        }

        public void Update(ApplicationUser entity)
        {
            _source[entity.Id] = entity;
        }
    }
}

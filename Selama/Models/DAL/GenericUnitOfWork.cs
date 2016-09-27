using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Models.DAL
{
    public class GenericUnitOfWork<TContext> : IGenericUnitOfWork
        where TContext : DbContext
    {
        protected TContext _context;

        public void Reload(object entity)
        {
            _context.Entry(entity).Reload();
        }
        public async Task ReloadAsync(object entity)
        {
            await _context.Entry(entity).ReloadAsync();
        }

        public void SaveChanges()
        {
            TrySaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await TrySaveChangesAsync();
        }

        public bool TrySaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    entity.Reload();
                }
            }
            catch (Exception e)
            {
            }

            // If we've reached here an error occurred
            return false;
        }
        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    await entity.ReloadAsync();
                }
            }
            catch (Exception e)
            {
            }

            // If we've reached here an error occurred
            return false;
        }

        private bool _isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing && _context != null)
                {
                    _context.Dispose();
                }
            }
            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
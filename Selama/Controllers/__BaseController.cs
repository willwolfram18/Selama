using Selama.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [RequireHttps]
    public class __BaseController : Controller
    {
        public const int HTTP_UNPROCESSABLE_ENTITY = 422;

        protected bool TrySaveChanges(DbContext db)
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected async Task<bool> TrySaveChangesAsync(DbContext db)
        {
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                ModelState.AddModelError("", "The data was updated before your changes could be saved.");
                foreach (var entity in concurrencyException.Entries)
                {
                    entity.Reload();
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        protected bool TrySaveChanges(DbContext db, out SaveChangeError result)
        {
            try
            {
                db.SaveChanges();
                result = SaveChangeError.None;
                return true;
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException)
                {
                    result = SaveChangeError.ConcurrencyError;
                    DbUpdateConcurrencyException concurrentError = e as DbUpdateConcurrencyException;
                    foreach (var entity in concurrentError.Entries)
                    {
                        entity.Reload();
                    }
                }
                else
                {
                    result = SaveChangeError.Unknown;
                }
                return false;
            }
        }

        protected void CompareProperties<TIn>(string propertyName, TIn dbValue, TIn modelValue, Func<TIn, TIn, bool> areEqualComp)
        {
            CompareProperties<TIn>(propertyName, dbValue, modelValue, "{0}", areEqualComp);
        }
        protected void CompareProperties<TIn>(string propertyName, TIn dbValue, TIn modelValue, string format, Func<TIn, TIn, bool> areEqualComp)
        {
            if (!areEqualComp(dbValue, modelValue))
            {
                ModelState.AddModelError(propertyName, string.Format("Current Value: " + format, dbValue));
            }
        }

        protected HttpStatusCodeResult HttpUnprocessable()
        {
            return new HttpStatusCodeResult(HTTP_UNPROCESSABLE_ENTITY);
        }
        protected HttpStatusCodeResult HttpUnprocessable(string description)
        {
            return new HttpStatusCodeResult(HTTP_UNPROCESSABLE_ENTITY, description);
        }
    }
}
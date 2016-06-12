using Selama.Classes.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [Authorize]
    // TODO: Enable HTTPS for _Base
    // [RequireHttps]
    public class _BaseAuthorizeController : Controller
    {
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
    }
}
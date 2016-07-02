﻿using Selama.Classes.Enum;
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
            catch (Exception ex)
            {
                if (ex is DbUpdateConcurrencyException)
                {
                    DbUpdateConcurrencyException concurrentError = ex as DbUpdateConcurrencyException;
                    foreach (var entity in concurrentError.Entries)
                    {
                        await entity.ReloadAsync();
                    }
                }
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

        protected ActionResult HttpUnprocessable()
        {
            return new HttpStatusCodeResult(422);
        }
        protected ActionResult HttpUnprocessable(string description)
        {
            return new HttpStatusCodeResult(422, description);
        }
    }
}
using Selama.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [RequireHttps]
    public class _ControllerBase : Controller
    {
        public static int HttpOkStatus
        {
            get
            {
                return (int)HttpStatusCode.OK;
            }
        }
        public static int HttpBadRequestStatus
        {
            get
            {
                return (int)HttpStatusCode.BadRequest;
            }
        }
        public static int HttpUnprocessableStatus
        {
            get
            {
                return 422;
            }
        }
        public static int HttpServerErrorStatus
        {
            get
            {
                return (int)HttpStatusCode.InternalServerError;
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

        protected HttpStatusCodeResult HttpBadRequest()
        {
            return new HttpStatusCodeResult(HttpBadRequestStatus);
        }
        protected HttpStatusCodeResult HttpBadRequest(string description)
        {
            return new HttpStatusCodeResult(HttpBadRequestStatus, description);
        }

        protected HttpStatusCodeResult HttpUnprocessable()
        {
            return new HttpStatusCodeResult(HttpUnprocessableStatus);
        }
        protected HttpStatusCodeResult HttpUnprocessable(string description)
        {
            return new HttpStatusCodeResult(HttpUnprocessableStatus, description);
        }
    }
}
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
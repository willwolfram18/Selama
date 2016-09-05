using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [AllowAnonymous]
    public class HomeController : _BaseAuthorizeController
    {
        public ViewResult Index()
        {
            BattleNetApiClient client = new BattleNetApiClient(ConfigurationManager.AppSettings["BattleNetOAuthClientId"]);

            return View();
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Join()
        {
            return View();
        }

        public ActionResult Error(string errorMsg = null)
        {
            if (string.IsNullOrWhiteSpace(errorMsg))
            {
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = errorMsg;
            return View("Error");
        }
    }
}
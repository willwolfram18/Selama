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
        public async Task<ActionResult> Index()
        {
            BattleNetApiClient client = new BattleNetApiClient(ConfigurationManager.AppSettings["BattleNetOAuthClientId"]);

            var a = await client.WowCommunityApi.GetItemClassesAsync();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Join()
        {
            ViewBag.Message = "Your contact page.";

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
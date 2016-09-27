using Selama.Models.DAL.Home;
using Selama.ViewModels.Home;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [AllowAnonymous]
    public class HomeController : _BaseAuthorizeController
    {
        private const int NEWS_FEED_SIZE = 25;

        public ViewResult Index()
        {
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

        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "page")]
        public async Task<ActionResult> GetGuildNewsFeed(int page = 1)
        {
            List<GuildNewsFeedViewModel> result = new List<GuildNewsFeedViewModel>();
            using (GuildNewsUnitOfWork db = new GuildNewsUnitOfWork())
            {
                if (User.Identity.IsAuthenticated)
                {
                    result = await db.GetMembersOnlyNews(page, NEWS_FEED_SIZE);
                }
                else
                {
                    result = await db.GetPublicGuildNews(page, NEWS_FEED_SIZE);
                }
            }

            if (result.Count == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return PartialView(result);
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
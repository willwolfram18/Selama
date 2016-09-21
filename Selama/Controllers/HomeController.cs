using Selama.Models.Home.DAL;
using Selama.ViewModels.Home;
using System;
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

        public async Task<ActionResult> GetGuildNewsFeed(int page = 0)
        {
            List<GuildNewsFeedViewModel> result = new List<GuildNewsFeedViewModel>();
            using (GuildNewsUnitOfWork db = new GuildNewsUnitOfWork())
            {
                result = await db.GetGuildNews(page, NEWS_FEED_SIZE);
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
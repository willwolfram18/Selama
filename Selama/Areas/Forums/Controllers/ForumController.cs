using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.ViewModels;
using Selama.Classes.Utility;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Controllers
{
    public class ForumController : Controller
    {
        public ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Forums/Forum
        public ActionResult Index()
        {
            List<ForumSectionViewModel> forums = Util.ConvertLists<ForumSection, ForumSectionViewModel>(
                _db.ForumSections.Where(f => f.IsActive).OrderBy(f => f.DisplayOrder),
                section => new ForumSectionViewModel(section)
            );
            
            return View(forums);
        }

        public ActionResult Threads(int id = 0)
        {
            Forum forum = _db.Forums.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }

            return View(new ForumViewModel(forum));
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
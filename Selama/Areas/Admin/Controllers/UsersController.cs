using Selama.Areas.Admin.ViewModels.Users;
using Selama.Classes.Attributes;
using Selama.Controllers;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Admin.Controllers
{
    [AuthorizePrivilege(Roles = "Admin,GuildOfficer")]
    public class UsersController : _BaseAuthorizeController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private const int _pageSize = 20;

        // GET: Admin/Users
        public async Task<ActionResult> Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction("Index", new { pageNum = page });
            }

            page--;
            var model = new UserOverviewViewModel(await _db.Users.OrderBy(u => u.UserName).ToListAsync(), _pageSize, page);
            if (page + 1 > model.NumPages)
            {
                return RedirectToAction("Index", new { pageNum = model.NumPages });
            }

            return View(model);
        }

        public async Task<ActionResult> DisableUser()
        {
            return View("Index");
        }

        public async Task<ActionResult> EnableUser()
        {
            return View("Index");
        }

        public async Task<ActionResult> ConfirmUser()
        {
            return View("Index");
        }

        public async Task<ActionResult> DenyUser()
        {
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
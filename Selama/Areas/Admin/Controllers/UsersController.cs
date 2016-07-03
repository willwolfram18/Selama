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

            if (TempData.ContainsKey("Message") && TempData["Message"] != null && 
                !string.IsNullOrWhiteSpace(TempData["Message"].ToString()))
            {
                ViewBag.Message = TempData["Message"].ToString();
                TempData["Message"] = null;
            }
            if (TempData.ContainsKey("ErrorMessage") && TempData["ErrorMessage"] != null &&
                !string.IsNullOrWhiteSpace(TempData["ErrorMessage"].ToString()))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData["ErrorMessage"] = null;
            }

            return View(model);
        }

        public async Task<ActionResult> DisableUser(UserEditViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && !dbUser.WaitingReview)
            {
                dbUser.IsActive = false;
                _db.Entry(dbUser).State = EntityState.Modified;
                if (await TrySaveChangesAsync(_db))
                {
                    TempData["Message"] = string.Format("<strong>{0}</strong> has been disabled and can no longer log in.", dbUser.UserName);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format("An error occurred. Please re-try disabling <strong>{0}</strong>.", dbUser.UserName);
                }
            }

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<ActionResult> EnableUser(UserEditViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && !dbUser.WaitingReview)
            {
                dbUser.IsActive = true;
                _db.Entry(dbUser).State = EntityState.Modified;
                if (await TrySaveChangesAsync(_db))
                {
                    TempData["Message"] = string.Format("<strong>{0}</strong> has been enabled and can now log in.", dbUser.UserName);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format("An error occurred. Please re-try enabling <strong>{0}</strong>.", dbUser.UserName);
                }
            }

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<ActionResult> ConfirmUser(UserEditViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && !dbUser.WaitingReview)
            {
                dbUser.WaitingReview = false;
                _db.Entry(dbUser).State = EntityState.Modified;
                if (await TrySaveChangesAsync(_db))
                {
                    TempData["Message"] = string.Format("<strong>{0}</strong> has been approved. A confirmation email has been sent to <strong>{1}</strong>.", 
                        dbUser.UserName, dbUser.Email);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format("An error occurred. Please re-try confirming <strong>{0}</strong>.", dbUser.UserName);
                }
            }

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<ActionResult> DenyUser(UserEditViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && !dbUser.WaitingReview)
            {
                string userName = dbUser.UserName;
                _db.Users.Remove(dbUser);
                _db.Entry(dbUser).State = EntityState.Deleted;
                if (await TrySaveChangesAsync(_db))
                {
                    TempData["Message"] = string.Format("<strong>{0}</strong> has been denied and deleted.", userName);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format("An error occurred. Please re-try denying <strong>{0}</strong>.", userName);
                }
            }

            return RedirectToAction("Index", new { page = page });
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
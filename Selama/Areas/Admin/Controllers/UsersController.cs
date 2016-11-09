using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Selama.Areas.Admin.ViewModels.Users;
using Selama.Common.Attributes;
using Selama.Common.Extensions;
using Selama.Common.Utility;
using Selama.Controllers;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Selama.Areas.Admin.Controllers
{
    [AuthorizePrivilege(Roles = "Admin,Guild Officer")]
    public class UsersController : _AuthorizeControllerBase
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableUser(UserStatusUpdateViewModel user, int page = 1)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableUser(UserStatusUpdateViewModel user, int page = 1)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmUser(UserStatusUpdateViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && dbUser.WaitingReview)
            {
                dbUser.WaitingReview = false;
                _db.Entry(dbUser).State = EntityState.Modified;
                if (await TrySaveChangesAsync(_db))
                {
                    // Send an email with this link
                    using (var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>())
                    {
                        string code = await userManager.GenerateEmailConfirmationTokenAsync(user.UserId);
                        var callbackUrl = Url.Action("ConfirmEmail", "Home", new { area = "Account", userId = user.UserId, code = code }, protocol: Request.Url.Scheme);
                        await userManager.SendEmailAsync(user.UserId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendEmail(UserStatusUpdateViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && dbUser.IsActive && !dbUser.EmailConfirmed)
            {
                // Send an email with this link
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>())
                {
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user.UserId);
                    var callbackUrl = Url.Action("ConfirmEmail", "Home", new { area = "Account", userId = user.UserId, code = code }, protocol: Request.Url.Scheme);
                    await userManager.SendEmailAsync(user.UserId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                }

                TempData["Message"] = string.Format("A new confirmation email has been sent to <strong>{0}</strong>.",
                    dbUser.Email);
            }

            return RedirectToAction("Index", new { page = page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DenyUser(UserStatusUpdateViewModel user, int page = 1)
        {
            ApplicationUser dbUser = await _db.Users.Where(u => u.Id == user.UserId).FirstOrDefaultAsync();
            if (ModelState.IsValid && dbUser != null && dbUser.WaitingReview)
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

        #region Edit
        public ActionResult Edit(string id)
        {
            ApplicationUser user = _db.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "A user with that ID does not exist";
                return RedirectToAction("Index");
            }
            else if (user.WaitingReview)
            {
                TempData["ErrorMessage"] = "<strong>" + user.UserName + "</strong> must be approved before you can edit them.";
                return RedirectToAction("Index");
            }

            _db.Roles.ToListOfDifferentType(
                r => new SelectListItem { Text = r.Name, Value = r.Id, Selected = (user.Roles.FirstOrDefault().RoleId == r.Id) }
            );
            
            return View(new UserEditViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel user)
        {
            ApplicationUser dbUser = _db.Users.Find(user.UserId);
            if (dbUser == null)
            {
                TempData["ErrorMessage"] = "A user with that ID does not exist";
                return RedirectToAction("Index");
            }
            else if (dbUser.WaitingReview)
            {
                TempData["ErrorMessage"] = "<strong>" + dbUser.UserName + "</strong> must be approved before you can edit them.";
                return RedirectToAction("Index");
            }

            user.ValidateModel(ModelState);
            user.EmailConfirmed = dbUser.EmailConfirmed;
            user.Username = dbUser.UserName;
            if (ModelState.IsValid)
            {
                await dbUser.UpdateFromViewModel(user);
                _db.Entry(dbUser).State = EntityState.Modified;
                if (await TrySaveChangesAsync(_db))
                {
                    TempData["Message"] = "<strong>" + dbUser.UserName + "</strong> has been updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    CompareProperties<bool>("IsActive", dbUser.IsActive, user.IsActive, (v1, v2) => v1 == v2);
                    var currentRole = _db.Roles.Find(dbUser.Roles.FirstOrDefault().RoleId);
                    var selectedRole = _db.Roles.Find(user.RoleId);
                    CompareProperties<string>("RoleId", currentRole.Name, selectedRole.Name, (v1, v2) => v1 == v2);
                    // Remove Version to force update on View
                    ModelState.Remove("Version");
                    user.Version = Convert.ToBase64String(dbUser.Version);
                }
            }

            ViewBag.RoleOptions = _db.Roles.ToListOfDifferentType(
                r => new SelectListItem { Text = r.Name, Value = r.Id, Selected = (user.RoleId == r.Id) }
            );
            return View(user);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
using Microsoft.AspNet.Identity;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Classes.Enum;
using Selama.Classes.Utility;
using Selama.Controllers;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Controllers
{
    public class ForumController : _BaseAuthorizeController
    {
        private ForumsUnitOfWork _db = new ForumsUnitOfWork();
        private const int PAGE_SIZE = 20;

        // GET: Forums/Forum
        public ActionResult Index(string redirectFrom = null)
        {
            List<ForumSectionViewModel> forums = Util.ConvertLists<ForumSection, ForumSectionViewModel>(
                _db.ForumSectionRepository.Get(f => f.IsActive, query => query.OrderBy(s => s.DisplayOrder)),
                section => new ForumSectionViewModel(section)
            );
            ViewBag.ErrorMsg = SelectErrorMessageFromRedirect(redirectFrom);

            return View(forums);
        }

        public async Task<ActionResult> Threads(int id = 0, int page = 1)
        {
            Forum forum = await _db.ForumRepository.FindByIdAsync(id);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Threads" });
            }
            if (page <= 0)
            {
                // Perform redirect to put URL page within [1,NumPages]
                return RedirectToAction("Threads", new { id = id, page = 1 });
            }

            // we want a zero-indexed page number on the server, but a one-indexed page client-side
            page--;
            ForumViewModel model = new ForumViewModel(forum, PAGE_SIZE, page);
            if (page > model.NumPages)
            {
                // Perform redirect to put URL page within [1,NumPages]
                return RedirectToAction("Threads", new { id = id, page = model.NumPages });
            }

            return View(model);
        }

        public async Task<ActionResult> Thread(int id = 0, int page = 1, string msg = null)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Thread" });
            }

            bool goToLastPage = page == -1;
            int pageSize = PAGE_SIZE;
            // if it's a non-positive page, redirect to 1st page (except if -1)
            if (page <= 0)
            {
                if (page == -1)
                {
                    page = 1;
                }
                else
                {
                    return RedirectToAction("Thread", new { id = id, page = 1 });
                }
            }

            // Subtract 1 on the first page to compensate for thread showing
            if (page == 1)
            {
                pageSize = PAGE_SIZE - 1;
            }
            page--;

            // Generate view model and redirect to last page if beyond
            ThreadViewModel viewModel = new ThreadViewModel(thread, pageSize, page);
            if (viewModel.ViewPageNum != page + 1 || goToLastPage)
            {
                return RedirectToAction("Thread", new { id = id, page = viewModel.NumPages });
            }
            ViewBag.Message = msg;
            return View(viewModel);
        }

        #region Create thread
        public async Task<ActionResult> CreateThread(int id = 0)
        {
            Forum forum = await _db.ForumRepository.FindByIdAsync(id);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index");
            }
            return View(new ThreadViewModel { ForumID = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateThread(ThreadViewModel thread, int id = 0)
        {
            Forum forum = await _db.ForumRepository.FindByIdAsync(id);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index");
            }

            thread.PostDate = DateTime.Now;
            thread.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                Thread dbThread = new Thread(thread, User.Identity.GetUserId(), id);
                _db.ThreadRepository.Add(dbThread);

                // Only admins and forum mods can pin and lock threads
                if (!Models.Thread.CanPinOrLockThreads(User))
                {
                    dbThread.IsLocked = dbThread.IsPinned = false;
                }

                if (await _db.TrySaveChangesAsync())
                {
                    return RedirectToAction("Thread", new { id = dbThread.ID });
                }
            }

            return View(thread);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostReply(ThreadReplyViewModel reply, int id = 0)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return HttpNotFound();
            }
            if (thread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            if (id != reply.ThreadID)
            {
                ModelState.AddModelError("ThreadID", "Invalid thread selected");
            }
            reply.PostDate = DateTime.Now;
            reply.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                ThreadReply dbReply = new ThreadReply(reply, User.Identity.GetUserId(), id, thread.Replies.Count + 1);
                _db.ThreadReplyRepository.Add(dbReply);
                if (await _db.TrySaveChangesAsync())
                {
                    dbReply.Author = await _db.IdentityRepository.FindByIdAsync(User.Identity.GetUserId());
                    Response.StatusCode = 200;

                    return PartialView("DisplayTemplates/ThreadReplyViewModel", new ThreadReplyViewModel(dbReply));
                }
            }

            // Generate the errors as a seriarlizable list of objects
            List<object> errors = new List<object>();
            foreach (var error in ModelState)
            {
                if (error.Value.Errors.Count > 0)
                {
                    var errorObj = new
                    {
                        Property = error.Key,
                        Errors = new List<string>()
                    };
                    foreach (var errorMsg in error.Value.Errors)
                    {
                        errorObj.Errors.Add(errorMsg.ErrorMessage);
                    }
                    errors.Add(errorObj);
                }
            }

            Response.StatusCode = 400; // Bad Request
            return Json(errors);
        }

        #region Thread editing
        public async Task<ActionResult> EditThread(int id = 0)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return HttpNotFound("Invalid ID");
            }
            else if (thread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            return PartialView("EditorTemplates/ThreadEditViewModel", new ThreadEditViewModel(thread));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditThread(ThreadEditViewModel thread)
        {
            Thread dbThread = await _db.ThreadRepository.FindByIdAsync(thread.ID);
            if (dbThread == null || !dbThread.IsActive)
            {
                return HttpNotFound("Invalid ID");
            }
            else if (dbThread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            thread.ValidateModel(ModelState);
            if (dbThread.AuthorID != User.Identity.GetUserId())
            {
                ModelState.AddModelError("", "You are not the author of this post");
            }
            if (ModelState.IsValid)
            {
                dbThread.UpdateFromViewModel(thread);

                if (await _db.TrySaveChangesAsync())
                {
                    await _db.ReloadAsync(dbThread);
                    return Json(new ThreadViewModel(dbThread, PAGE_SIZE, 0).HtmlContent.ToString());
                }
            }

            return HttpUnprocessable();
        }
        #endregion

        #region Reply editing
        public async Task<ActionResult> EditReply(int id = 0)
        {
            ThreadReply reply = await _db.ThreadReplyRepository.FindByIdAsync(id);
            if (reply == null || !reply.IsActive)
            {
                return HttpNotFound("Invalid ID");
            }
            if (reply.Thread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            return PartialView("EditorTemplates/ThreadReplyEditViewModel", new ThreadReplyEditViewModel(reply));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditReply(ThreadReplyEditViewModel reply)
        {
            ThreadReply dbReply = await _db.ThreadReplyRepository.FindByIdAsync(reply.ID);
            if (reply == null || !dbReply.IsActive)
            {
                return HttpNotFound("Invalid ID");
            }
            if (dbReply.Thread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            reply.ValidateModel(ModelState);
            if (dbReply.ThreadID != reply.ThreadID)
            {
                ModelState.AddModelError("ThreadID", "Invalid Thread ID");
            }
            if (dbReply.AuthorID != User.Identity.GetUserId())
            {
                ModelState.AddModelError("", "You are not the author of this post, and therfore cannot edit it");
            }
            if (ModelState.IsValid)
            {
                dbReply.UpdateFromViewModel(reply);
                if (await _db.TrySaveChangesAsync())
                {
                    await _db.ReloadAsync(dbReply);
                    return Json(new { id = dbReply.ID, content = new ThreadReplyViewModel(dbReply).HtmlContent.ToString() });
                }
            }

            return HttpUnprocessable();
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteThread(int id = 0, int page = 1)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Thread" });
            }
            if (thread.IsLocked)
            {
                return RedirectToAction("Thread", new { id = thread.ID, page = page, msg = "This thread is locked from editing and cannot be deleted" });
            }
            if (thread.AuthorID != User.Identity.GetUserId())
            {
                return RedirectToAction("Thread", new { id = thread.ID, page = page, msg = "You are not the author of this thread and cannot delete it" });
            }

            // Mark thread "deleted"
            thread.IsActive = false;
            _db.ThreadRepository.Update(thread);
            // Mark all replies for thread as "deleted"
            foreach (ThreadReply reply in thread.Replies)
            {
                reply.IsActive = false;
                _db.ThreadReplyRepository.Update(reply);
            }
            if (await _db.TrySaveChangesAsync())
            {
                return RedirectToAction("Threads", new { id = thread.ForumID });
            }
            return RedirectToAction("Thread", new { id = id, page = page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteReply(int id = 0, int threadId = 0, int page = 1)
        {
            ThreadReply reply = await _db.ThreadReplyRepository.FindByIdAsync(id);
            if (reply == null || reply.ThreadID != threadId || !reply.Thread.IsActive)
            {
                return HttpNotFound();
            }
            if (reply.Thread.IsLocked)
            {
                return RedirectToAction("Thread", new { id = threadId, page = page, msg = "The thread is locked for editing and therefore the reply cannot be deleted" });
            }
            if (reply.AuthorID != User.Identity.GetUserId())
            {
                return RedirectToAction("Thread", new { id = threadId, page = page, msg = "You are not the author of this reply and cannot delete it" });
            }

            reply.IsActive = false;
            _db.ThreadReplyRepository.Update(reply);
            await _db.SaveChangesAsync();
            return RedirectToAction("Thread", new { id = threadId, page = page });
        }

        #region Lock thread
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LockThread(int id = 0, int page = 1)
        {
            return await SetThreadLock(id, page, true, "locking");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnlockThread(int id = 0, int page = 1)
        {
            return await SetThreadLock(id, page, false, "unlocking");
        }

        private async Task<ActionResult> SetThreadLock(int id, int page, bool locking, string lockWord)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index");
            }

            string message = null;
            if (Models.Thread.CanPinOrLockThreads(User))
            {
                thread.IsLocked = locking;
                _db.ThreadRepository.Update(thread);
                if (!await _db.TrySaveChangesAsync())
                {
                    message = "There was an error " + lockWord + " the thread. Please try again.";
                }
            }

            return RedirectToAction("Thread", new { id = id, page = page, msg = message });
        }
        #endregion

        #region Pin thread
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PinThread(int id = 0, int page = 1)
        {
            return await SetThreadPin(id, page, true, "pinning");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnpinThread(int id = 0, int page = 1)
        {
            return await SetThreadPin(id, page, false, "unpinning");
        }

        private async Task<ActionResult> SetThreadPin(int id, int page, bool pinning, string pinningWord)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index");
            }

            string message = null;
            if (Models.Thread.CanPinOrLockThreads(User))
            {
                thread.IsPinned = pinning;
                _db.ThreadRepository.Update(thread);
                if (!await _db.TrySaveChangesAsync())
                {
                    message = "There was an error " + pinningWord + " the thread. Please try again.";
                }
            }

            return RedirectToAction("Thread", new { id = id, page = page, msg = message });
        }
        #endregion

        public async Task<ActionResult> GetThreadQuote(int id = 0)
        {
            Thread thread = await _db.ThreadRepository.FindByIdAsync(id);
            if (thread == null || !thread.IsActive)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return PartialView("DisplayTemplates/ThreadReplyQuoteViewModel", new ThreadReplyQuoteViewModel(thread));
        }

        public async Task<ActionResult> GetReplyQuote(int id = 0, int page = 1)
        {
            ThreadReply reply = await _db.ThreadReplyRepository.FindByIdAsync(id);
            if (reply == null || !reply.IsActive)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return PartialView("DisplayTemplates/ThreadReplyQuoteViewModel", new ThreadReplyQuoteViewModel(reply, page));
        }

        private string SelectErrorMessageFromRedirect(string redirectedFrom)
        {
            switch (redirectedFrom)
            {
                case "":
                case null:
                    return null;
                case "Threads":
                    return "That forum does not exist";
                case "Thread":
                    return "That thread does not exist";
                default:
                    throw new NotImplementedException(string.Format("No case implemented for redirect from: \"{0}\"", redirectedFrom));
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
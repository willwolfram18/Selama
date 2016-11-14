using Microsoft.AspNet.Identity;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Common.Extensions;
using Selama.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Controllers
{
    [Route("{action=index}")]
    public class ForumController : _ForumsControllerBase
    {
        private IForumsUnitOfWork _db;
        public const int PAGE_SIZE = 20;

        public ForumController()
        {
            _db = new ForumsUnitOfWork();
        }

        public ForumController(IForumsUnitOfWork db)
        {
            _db = db;
        }

        // GET: Forums
        public ActionResult Index(string redirectFrom = null)
        {
            List<ForumSectionViewModel> forums = _db.ForumSections.Get(f => f.IsActive).ToListOfDifferentType(
                section => new ForumSectionViewModel(section)
            );
            ViewBag.ErrorMsg = SelectErrorMessageFromRedirect(redirectFrom);

            return View(forums);
        }

        // GET: Forums/Threads/3
        [Route("{id:int}/threads")]
        public async Task<ActionResult> Threads([Bind(Prefix = "id")] int forumId = 0, int page = 1)
        {
            Forum forum = await _db.Forums.FindByIdAsync(forumId);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Threads" });
            }
            if (page <= 0)
            {
                // Perform redirect to put URL page within [1,NumPages]
                return RedirectToAction("Threads", new { id = forumId, page = 1 });
            }

            // we want a zero-indexed page number on the server, but a one-indexed page client-side
            int zeroIndexedPage = page - 1;
            ForumViewModel model = new ForumViewModel(forum, PAGE_SIZE, zeroIndexedPage);
            if (zeroIndexedPage >= model.NumPages)
            {
                // Perform redirect to put URL page within [1,NumPages]
                return RedirectToAction("Threads", new { id = forumId, page = model.NumPages });
            }

            return View(model);
        }

        // GET: Forums/Thread/2
        [Route("thread/{id:int}")]
        public async Task<ActionResult> Thread([Bind(Prefix = "id")] int threadId = 0, int page = 1, string msg = null)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
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
                    return RedirectToAction("Thread", new { id = threadId, page = 1 });
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
                return RedirectToAction("Thread", new { id = threadId, page = viewModel.NumPages });
            }
            ViewBag.Message = msg;
            return View(viewModel);
        }

        #region Create thread
        // GET: /Forums/CreateThread/3
        [Route("{id:int}/CreateThread")]
        public async Task<ActionResult> CreateThread([Bind(Prefix = "id")] int forumId = 0)
        {
            Forum forum = await _db.Forums.FindByIdAsync(forumId);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index");
            }
            return View(new ThreadViewModel { ForumID = forumId });
        }

        // POST: /Forums/CreateThread/3
        [HttpPost]
        [Route("{id:int}/CreateThread")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateThread(ThreadViewModel thread, [Bind(Prefix = "id")] int forumId = 0)
        {
            Forum forum = await _db.Forums.FindByIdAsync(forumId);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index");
            }

            thread.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                Thread dbThread = _db.CreateNewThread(thread, User, forumId);

                if (await _db.TrySaveChangesAsync())
                {
                    await _db.AddNewThreadToNewsFeedAsync(dbThread,
                        Url.Action("Thread", new { id = dbThread.Id }));
                    return RedirectToAction("Thread", new { id = dbThread.Id });
                }
            }

            return View(thread);
        }
        #endregion

        // POST: Forums/PostReply/2
        [HttpPost]
        [Route("thread/{id:int}/reply")]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> PostReply(ThreadReplyViewModel reply, [Bind(Prefix = "id")] int threadId = 0)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (thread == null || !thread.IsActive || thread.IsLocked || threadId != reply.ThreadID)
            {
                ModelState.AddModelError("", "You cannot post to that thread");
            }

            reply.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                ThreadReply dbReply = new ThreadReply(reply, User.Identity.GetUserId(), thread);
                _db.ThreadReplies.Add(dbReply);
                if (await _db.TrySaveChangesAsync())
                {
                    dbReply.Author = await _db.Authors.FindByIdAsync(User.Identity.GetUserId());

                    Response.StatusCode = HttpOkStatus;
                    return PartialView("DisplayTemplates/ThreadReplyViewModel", new ThreadReplyViewModel(dbReply));
                }
            }

            Response.StatusCode = _ControllerBase.HttpBadRequestStatus;
            return PartialView("EditorTemplates/ThreadReplyViewModel", reply);
        }

        #region Thread editing
        // GET: Forums/EditThread/2
        [Route("thread/{id:int}/edit")]
        public async Task<PartialViewResult> EditThread([Bind(Prefix = "id")] int threadId = 0)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (UserCanEditThread(thread))
            {
                Response.StatusCode = HttpBadRequestStatus;
                ViewBag.ThreadAction = "edit";
                return PartialView("_InvalidThread");
            }

            return PartialView("EditorTemplates/ThreadEditViewModel", new ThreadEditViewModel(thread));
        }

        // POST: Forums/EditThread/2
        [HttpPost]
        [Route("thread/{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditThread(ThreadEditViewModel thread)
        {
            Thread dbThread = await _db.Threads.FindByIdAsync(thread.ID);
            if (UserCanEditThread(dbThread))
            {
                ModelState.AddModelError("", "You cannot edit this post");
            }

            thread.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                dbThread.UpdateFromViewModel(thread);

                if (await _db.TrySaveChangesAsync())
                {
                    await _db.ReloadAsync(dbThread);
                    return PartialView("DisplayTemplates/ThreadPostViewModel", new ThreadPostViewModel(
                        new ThreadViewModel(dbThread, PAGE_SIZE, 0)
                    ));
                }
            }

            Response.StatusCode = HttpBadRequestStatus;
            return PartialView("EditorTemplates/ThreadEditViewModel");
        }
        #endregion

        #region Reply editing
        // GET: Forums/EditReply/2
        [Route("reply/edit")]
        public async Task<ActionResult> EditReply([Bind(Prefix = "id")] int replyId = 0)
        {
            ThreadReply reply = await _db.ThreadReplies.FindByIdAsync(replyId);
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

        // POST: Forums/EditReply/2
        [HttpPost]
        [Route("reply/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditReply(ThreadReplyEditViewModel reply)
        {
            ThreadReply dbReply = await _db.ThreadReplies.FindByIdAsync(reply.ID);
            if (reply == null || !dbReply.IsActive)
            {
                return HttpNotFound("Invalid ID");
            }
            if (dbReply.Thread.IsLocked)
            {
                return HttpUnprocessable("Thread is locked");
            }

            reply.ValidateModel(ModelState);
            if (dbReply.ThreadId != reply.ThreadID)
            {
                ModelState.AddModelError("ThreadID", "Invalid Thread ID");
            }
            // Only the author can edit
            if (dbReply.AuthorId != User.Identity.GetUserId())
            {
                ModelState.AddModelError("", "You are not the author of this post, and therfore cannot edit it");
            }
            if (ModelState.IsValid)
            {
                dbReply.UpdateFromViewModel(reply);
                if (await _db.TrySaveChangesAsync())
                {
                    await _db.ReloadAsync(dbReply);
                    return Json(new { id = dbReply.Id, content = new ThreadReplyViewModel(dbReply).HtmlContent.ToString() });
                }
            }

            // If we reach here there was an error
            return HttpUnprocessable();
        }
        #endregion

        // POST: Forums/DeleteThread/2
        [HttpPost]
        [Route("thread/{id:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> DeleteThread([Bind(Prefix = "id")] int threadId = 0, int page = 1)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Thread" });
            }
            // Cannot delete a thread that is locked
            if (thread.IsLocked)
            {
                return RedirectToAction("Thread", new { id = thread.Id, page = page, msg = "This thread is locked from editing and cannot be deleted" });
            }
            // Cannot delete a thread that isn't yours
            if (thread.AuthorId != User.Identity.GetUserId())
            {
                return RedirectToAction("Thread", new { id = thread.Id, page = page, msg = "You are not the author of this thread and cannot delete it" });
            }

            // Mark thread "deleted"
            _db.DeleteThread(thread);
            if (await _db.TrySaveChangesAsync())
            {
                return RedirectToAction("Threads", new { id = thread.ForumId });
            }
            return RedirectToAction("Thread", new { id = threadId, page = page });
        }

        [HttpPost]
        [Route("thread/{threadId:int}/reply-delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteReply([Bind(Prefix = "id")] int replyId = 0, int threadId = 0, int page = 1)
        {
            ThreadReply reply = await _db.ThreadReplies.FindByIdAsync(replyId);
            if (reply == null || reply.ThreadId != threadId || !reply.Thread.IsActive)
            {
                return HttpNotFound();
            }
            // Cannot delete a reply of a within a locked thread
            if (reply.Thread.IsLocked)
            {
                return RedirectToAction("Thread", new { id = threadId, page = page, msg = "The thread is locked for editing and therefore the reply cannot be deleted" });
            }
            // Cannot delete a thread you do not own
            if (reply.AuthorId != User.Identity.GetUserId())
            {
                return RedirectToAction("Thread", new { id = threadId, page = page, msg = "You are not the author of this reply and cannot delete it" });
            }

            reply.IsActive = false;
            _db.ThreadReplies.Update(reply);
            await _db.SaveChangesAsync();
            return RedirectToAction("Thread", new { id = threadId, page = page });
        }

        #region Lock thread
        // POST: Forums/LockThread/2
        [HttpPost]
        [Route("thread/{id:int}/lock")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LockThread([Bind(Prefix = "id")] int threadId = 0, int page = 1)
        {
            return await SetThreadLock(threadId, page, true, "locking");
        }

        // POST: Forums/UnlockThread/2
        [HttpPost]
        [Route("thread/{id:int}/unlock")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnlockThread([Bind(Prefix = "id")] int threadId = 0, int page = 1)
        {
            return await SetThreadLock(threadId, page, false, "unlocking");
        }

        private async Task<ActionResult> SetThreadLock(int threadId, int page, bool locking, string lockWord)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index");
            }

            string message = null;
            if (Models.Thread.CanPinOrLockThreads(User))
            {
                thread.IsLocked = locking;
                _db.Threads.Update(thread);
                if (!await _db.TrySaveChangesAsync())
                {
                    message = "There was an error " + lockWord + " the thread. Please try again.";
                }
            }

            return RedirectToAction("Thread", new { id = threadId, page = page, msg = message });
        }
        #endregion

        #region Pin thread
        // POST: Forums/PinThread/2
        [HttpPost]
        [Route("thread/{id:int}/pin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PinThread([Bind(Prefix = "id")] int threadId = 0, int page = 1)
        {
            return await SetThreadPin(threadId, page, true, "pinning");
        }

        // POST: Forums/UnpinThread/2
        [HttpPost]
        [Route("thread/{id:int}/unpin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnpinThread([Bind(Prefix = "id")] int threadId = 0, int page = 1)
        {
            return await SetThreadPin(threadId, page, false, "unpinning");
        }

        private async Task<ActionResult> SetThreadPin(int threadId, int page, bool pinning, string pinningWord)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index");
            }

            string message = null;
            if (Models.Thread.CanPinOrLockThreads(User))
            {
                thread.IsPinned = pinning;
                _db.Threads.Update(thread);
                if (!await _db.TrySaveChangesAsync())
                {
                    message = "There was an error " + pinningWord + " the thread. Please try again.";
                }
            }

            return RedirectToAction("Thread", new { id = threadId, page = page, msg = message });
        }
        #endregion

        // GET: Forums/GetThreadQuote/2
        [Route("thread/{id:int}/quote")]
        public async Task<ActionResult> GetThreadQuote([Bind(Prefix = "id")] int threadId = 0)
        {
            Thread thread = await _db.Threads.FindByIdAsync(threadId);
            if (thread == null || !thread.IsActive)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return PartialView("DisplayTemplates/ThreadReplyQuoteViewModel", new ThreadReplyQuoteViewModel(thread));
        }

        // GET: Forums/GetReplyQuote/4
        [Route("reply/quote")]
        public async Task<ActionResult> GetReplyQuote([Bind(Prefix = "id")] int replyId = 0, int page = 1)
        {
            ThreadReply reply = await _db.ThreadReplies.FindByIdAsync(replyId);
            if (reply == null || !reply.IsActive)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return PartialView("DisplayTemplates/ThreadReplyQuoteViewModel", new ThreadReplyQuoteViewModel(reply, page));
        }

        #region Protected methods
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        #region Private methods
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

        private bool UserCanEditThread(Thread thread)
        {
            return thread == null || !thread.IsActive || thread.IsLocked || thread.AuthorId != User.Identity.GetUserId();
        }
        #endregion
    }
}
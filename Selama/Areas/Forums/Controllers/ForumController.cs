using Microsoft.AspNet.Identity;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.ViewModels;
using Selama.Classes.Enum;
using Selama.Classes.Utility;
using Selama.Controllers;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Controllers
{
    public class ForumController : _BaseAuthorizeController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private const int _pageSize = 20;

        // GET: Forums/Forum
        public ActionResult Index(string redirectFrom = null)
        {
            List<ForumSectionViewModel> forums = Util.ConvertLists<ForumSection, ForumSectionViewModel>(
                _db.ForumSections.Where(f => f.IsActive).OrderBy(f => f.DisplayOrder),
                section => new ForumSectionViewModel(section)
            );
            if (redirectFrom == "Threads")
            {
                ViewBag.ErrorMsg = "That forum does not exist";
            }
            else if (redirectFrom == "Thread")
            {
                ViewBag.ErrorMsg = "That thread does not exist";
            }
            return View(forums);
        }

        public ActionResult Threads(int id = 0)
        {
            Forum forum = _db.Forums.Find(id);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Threads" });
            }

            return View(new ForumViewModel(forum));
        }

        public ActionResult Thread(int id = 0, int page = 1, string msg = null)
        {
            Thread thread = _db.Threads.Find(id);
            if (thread == null || !thread.IsActive)
            {
                return RedirectToAction("Index", new { redirectFrom = "Thread" });
            }

            // Subtract 1 on the first page to compensate for thread showing
            int pageSize = _pageSize;
            if (page <= 0)
            {
                return RedirectToAction("Thread", new { id = id, page = 1 });
            }
            if (page == 1)
            {
                pageSize = _pageSize - 1;
            }
            page--;

            // Generate view model and redirect to last page if beyond
            ThreadViewModel viewModel = new ThreadViewModel(thread, pageSize, page);
            if (viewModel.ViewPageNum != page + 1)
            {
                return RedirectToAction("Thread", new { id = id, page = viewModel.ViewPageNum });
            }
            ViewBag.Message = msg;
            return View(viewModel);
        }

        #region Create thread
        public ActionResult CreateThread(int id = 0)
        {
            Forum forum = _db.Forums.Find(id);
            if (forum == null || !forum.IsActive)
            {
                return RedirectToAction("Index");
            }
            return View(new ThreadViewModel { ForumID = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateThread(ThreadViewModel thread, int id = 0)
        {
            Forum forum = _db.Forums.Where(f => f.IsActive && f.ID == id).FirstOrDefault();
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            thread.PostDate = DateTime.Now;
            thread.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                _db.Threads.Add(new Thread(thread, User.Identity.GetUserId(), id));
                SaveChangeError result;
                if (TrySaveChanges(_db, out result))
                {
                    return RedirectToAction("Thread", new { id = id });
                }
            }

            return View(thread);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostReply(ThreadReplyViewModel reply, int id = 0)
        {
            Thread thread = _db.Threads.Find(id);
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
                _db.ThreadReplies.Add(dbReply);
                if (TrySaveChanges(_db))
                {
                    dbReply.Author = _db.Users.Find(User.Identity.GetUserId());
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
        public ActionResult EditThread(int id = 0)
        {
            Thread thread = _db.Threads.Find(id);
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
        public ActionResult EditThread(ThreadEditViewModel thread)
        {
            Thread dbThread = _db.Threads.Find(thread.ID);
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
                if (TrySaveChanges(_db))
                {
                    _db.Entry(dbThread).Reload();
                    return Json(new ThreadViewModel(dbThread, _pageSize, 0).HtmlContent.ToString());
                }
            }

            return HttpUnprocessable();
        }
        #endregion

        #region Reply editing
        public ActionResult EditReply(int id = 0)
        {
            ThreadReply reply = _db.ThreadReplies.Find(id);
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
        public ActionResult EditReply(ThreadReplyEditViewModel reply)
        {
            ThreadReply dbReply = _db.ThreadReplies.Find(reply.ID);
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
                if (TrySaveChanges(_db))
                {
                    _db.Entry(dbReply).Reload();
                    return Json(new { id = dbReply.ID, content = new ThreadReplyViewModel(dbReply).HtmlContent.ToString() });
                }
            }

            return HttpUnprocessable();
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteThread(int id = 0, int page = 1)
        {
            Thread thread = _db.Threads.Find(id);
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
            _db.Entry(thread).State = System.Data.Entity.EntityState.Modified;
            // Mark all replies for thread as "deleted"
            foreach (ThreadReply reply in thread.Replies)
            {
                reply.IsActive = false;
                _db.Entry(reply).State = System.Data.Entity.EntityState.Modified;
            }
            if (TrySaveChanges(_db))
            {
                return RedirectToAction("Threads", new { id = thread.ForumID });
            }
            return RedirectToAction("Thread", new { id = id, page = page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReply(int id = 0, int threadId = 0, int page = 1)
        {
            ThreadReply reply = _db.ThreadReplies.Find(id);
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
            _db.Entry(reply).State = System.Data.Entity.EntityState.Modified;
            TrySaveChanges(_db);
            return RedirectToAction("Thread", new { id = threadId, page = page });
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
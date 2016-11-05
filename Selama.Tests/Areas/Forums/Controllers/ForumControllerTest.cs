using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using Selama.Tests.Areas.Forums.Models.DAL;
using Selama.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Tests.Areas.Forums.Controllers
{
    [TestClass]
    public class ForumControllerTest : _BaseControllerUnitTest<ForumController>
    {
        #region Properties
        #region Private properties
        private const int NUM_AUTHORS = 2;
        private const int NUM_FORUM_SECTIONS = 2;
        private const int NUM_FORUMS = 2 * NUM_FORUM_SECTIONS;
        private const int NUM_THREADS = NUM_FORUMS * 3;

        private MockForumsUnitOfWork ControllerDb { get; set; }
        #endregion
        #endregion

        #region Setup tests
        protected override ForumController SetupController()
        {
            ControllerDb = new MockForumsUnitOfWork();
            return new ForumController(ControllerDb);
        }
        protected override void AdditionalSetup()
        {
            SetupAuthors();
            SetupForumSections();
            SetupForums();
            SetupThreads();
        }
        #endregion

        #region Methods
        #region Public methods
        [TestMethod]
        public void IndexIsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Index() as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            #endregion
        }

        #region Threads unit tests
        [TestMethod]
        public async Task ThreadsActionWithoutForumIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithInvalidThreadIdRedirectToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumLessThanOneRedirectsToPageOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 0) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToActionPageOne(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            // Arrange
            int forumId = 1;
            Forum forum = ControllerDb.Forums.FindById(forumId);
            ApplicationUser author = ControllerDb.Authors.Get().ToList()[0];
            for (int i = 0; i < ForumController.PAGE_SIZE; i++)
            {
                Thread tempThread = new Thread
                {
                    Content = string.Format("A sample paragraph for thread {0} in forum {1}", i, forumId),
                    IsActive = true,
                    IsLocked = false,
                    IsPinned = false,
                    PostDate = DateTime.Now,
                    Title = "Extra Thread " + i.ToString(),
                    Author = author,
                    AuthorId = author.Id,
                    Forum = forum,
                    ForumId = forum.Id,
                    Replies = new List<ThreadReply>(),
                };
                ControllerDb.Threads.Add(tempThread);
                forum.Threads.Add(tempThread);
            }

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 3) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToSpecificActionPage(result, "Threads", 2);
        }

        [TestMethod]
        public async Task ThreadsWithInactiveForumRedirectsToIndex()
        {
            // Arrange
            ControllerDb.Forums.FindById(1).IsActive = false;

            // Act
            RedirectToRouteResult result = await Controller.Threads(1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithValidIdReturnsCorrectResult()
        {
            // Arrange

            // Act
            ViewResult result = await Controller.Threads(1) as ViewResult;

            // Assert
            ForumViewModel Model = result.Model as ForumViewModel;
            Assert.IsNotNull(result);
            Assert.IsNotNull(Model);
            Assert.AreEqual(ControllerDb.Forums.FindById(1).Title, Model.Title);
        }

        /* TODO:
         *   - Forum is active and no threads, check model's threads
         *   - Forum is inactive
         *   - Forum with inactive threads
         */
        #endregion

        #region Thread unit tests
        [TestMethod]
        public async Task ThreadWithoutIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread() as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithInvalidIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithInactiveThreadResultsInRedirectToIndex()
        {
            // Arrange
            int threadId = 1;
            ControllerDb.Threads.FindById(threadId).IsActive = false;

            // Act
            RedirectToRouteResult result = await Controller.Thread(threadId) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithBelowOnePageNumRedirectsToPageOne()
        {
            #region Arrange
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(1, 0) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToActionPageOne(result, "Thread");
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            #region Arrange
            int threadId = 1;
            Thread t = ControllerDb.Threads.FindById(threadId);
            for (int i = 0; i < ForumController.PAGE_SIZE + 2; i++)
            {
                InsertReplyToThread(t, i);
            }
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(threadId, 3) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToSpecificActionPage(result, "Thread", 2);
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithNegativeOnePageNumberRedirectsToLastPage()
        {
            #region Arrange
            int threadId = 1;
            Thread t = ControllerDb.Threads.FindById(threadId);
            for (int i = 0; i < ForumController.PAGE_SIZE + 2; i++)
            {
                InsertReplyToThread(t, i);
            }
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(threadId, -1) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToSpecificActionPage(result, "Thread", 2);
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithValidIdReturnsCorrectResult()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = await Controller.Thread(1) as ViewResult;
            #endregion

            #region Assert
            ThreadViewModel Model = result.Model as ThreadViewModel;
            Assert.IsNotNull(result);
            Assert.IsNotNull(Model);
            Assert.AreEqual(ControllerDb.Threads.FindById(1).Title, Model.Title);
            Assert.AreEqual(ControllerDb.Threads.FindById(1).Content, Model.Content);
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private void SetupAuthors()
        {
            for (int i = 0; i < NUM_AUTHORS; i++)
            {
                ControllerDb.Authors.Add(new ApplicationUser
                {
                    IsActive = true,
                    UserName = "App User " + i.ToString(),
                    Email = string.Format("appuser{0}@email.com", i),
                    EmailConfirmed = true,
                    ThreadReplies = new List<ThreadReply>(),
                    WaitingReview = false,
                    Threads = new List<Thread>(),
                });
            }
        }
        private void SetupForumSections()
        {
            for (int i = 0; i < NUM_FORUM_SECTIONS; i++)
            {
                ControllerDb.ForumSections.Add(new ForumSection
                {
                    DisplayOrder = i,
                    IsActive = true,
                    Title = "Forum Section " + i.ToString(),
                    Forums = new List<Forum>(),
                });
            }
        }
        private void SetupForums()
        {
            for (int i = 0; i < NUM_FORUMS; i++)
            {
                int forumSectionId = (i % NUM_FORUM_SECTIONS) + 1;
                ForumSection section = ControllerDb.ForumSections.FindById(forumSectionId);
                Forum forum = new Forum
                {
                    ForumSectionId = forumSectionId,
                    ForumSection = section,
                    IsActive = true,
                    Title = "Forum " + i.ToString(),
                    SubTitle = "Subtitle for forum " + i.ToString(),
                    Threads = new List<Thread>(),
                };
                section.Forums.Add(forum);
                ControllerDb.Forums.Add(forum);
            }
        }
        private void SetupThreads()
        {
            for (int i = 0; i < NUM_THREADS; i++)
            {
                int forumId = (i % NUM_FORUMS) + 1;
                Forum forum = ControllerDb.Forums.FindById(forumId);
                ApplicationUser author = ControllerDb.Authors.Get().ToList()[i % NUM_AUTHORS];
                Thread thread = new Thread
                {
                    Content = string.Format("This is a sample paragraph for thread {0} in forum {1}.", i, forumId),
                    Forum = forum,
                    ForumId = forumId,
                    IsActive = true,
                    IsLocked = false,
                    IsPinned = false,
                    PostDate = DateTime.Now,
                    Title = "Thread " + i.ToString(),
                    Replies = new List<ThreadReply>(),
                    Author = author,
                    AuthorId = author.Id
                };
                forum.Threads.Add(thread);
                ControllerDb.Threads.Add(thread);
            }
        }

        private void AssertIsRedirectToIndex(RedirectToRouteResult result, string redirectFromAction)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual(redirectFromAction, result.RouteValues["redirectFrom"].ToString());
        }

        private void AssertIsRedirectToActionPageOne(RedirectToRouteResult result, string actionRedirectTarget)
        {
            AssertIsRedirectToSpecificActionPage(result, actionRedirectTarget, 1);
        }
        private void AssertIsRedirectToSpecificActionPage(RedirectToRouteResult result, string actionRedirectTarget, int targetPageNum)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(actionRedirectTarget, result.RouteValues["action"].ToString());
            Assert.AreEqual(targetPageNum, Convert.ToInt32(result.RouteValues["page"]));
        }

        private void InsertReplyToThread(Thread thread, int replyIndex)
        {
            var reply = new ThreadReply
            {
                Thread = thread,
                ThreadId = thread.Id,
                Author = thread.Author,
                AuthorId = thread.AuthorId,
                Content = "Reply " + replyIndex + " to thread " + thread.Title,
                IsActive = true,
                PostDate = DateTime.Now,
                ReplyIndex = replyIndex,
            };
            thread.Replies.Add(reply);
            ControllerDb.ThreadReplies.Add(reply);
        }
        #endregion
        #endregion
    }
}

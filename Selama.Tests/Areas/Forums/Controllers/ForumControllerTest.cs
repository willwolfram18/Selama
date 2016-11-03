using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
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
        private const int NUM_AUTHORS = 2;
        private const int NUM_FORUM_SECTIONS = 2;
        private const int NUM_FORUMS = 2 * NUM_FORUM_SECTIONS;
        private const int NUM_THREADS = NUM_FORUMS * 3;

        private MockForumsUnitOfWork controllerDb = new MockForumsUnitOfWork();

        protected override ForumController SetupController()
        {
            return new ForumController(controllerDb);
        }

        protected override void AdditionalSetup()
        {
            SetupAuthors();
            SetupForumSections();
            SetupForums();
            SetupThreads();
        }

        private void SetupAuthors()
        {
            for (int i = 0; i < NUM_AUTHORS; i++)
            {
                controllerDb.Authors.Add(new ApplicationUser
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
                controllerDb.ForumSections.Add(new ForumSection
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
                ForumSection section = controllerDb.ForumSections.FindById(forumSectionId);
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
                controllerDb.Forums.Add(forum);
            }
        }
        private void SetupThreads()
        {
            for (int i = 0; i < NUM_THREADS; i++)
            {
                int forumId = (i % NUM_FORUMS) + 1;
                Forum forum = controllerDb.Forums.FindById(forumId);
                ApplicationUser author = controllerDb.Authors.Get().ToList()[i % NUM_AUTHORS];
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
                controllerDb.Threads.Add(thread);
            }
        }

        [TestMethod]
        public void IndexIsNotNull()
        {
            // Arrange

            // Act
            ViewResult result = Controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #region Threads
        [TestMethod]
        public async Task ThreadsIsNotNull()
        {
            // Arrange

            // Act
            ViewResult result = await Controller.Threads(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ThreadsActionWithoutForumIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual("Threads", result.RouteValues["redirectFrom"].ToString());
        }

        [TestMethod]
        public async Task ThreadsWithInvalidThreadIdRedirectToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual("Threads", result.RouteValues["redirectFrom"].ToString());
        }

        [TestMethod]
        public async Task ThreadsWithPageNumLessThanOneRedirectsToPageOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Threads", result.RouteValues["action"].ToString());
            Assert.AreEqual(1, Convert.ToInt32(result.RouteValues["page"]));
        }

        [TestMethod]
        public async Task ThreadsWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            // Arrange
            // TODO: Need to add more threads to check proper page number in redirect

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 2) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Threads", result.RouteValues["action"].ToString());
            Assert.AreEqual(1, Convert.ToInt32(result.RouteValues["page"]));
        }
        #endregion
    }
}

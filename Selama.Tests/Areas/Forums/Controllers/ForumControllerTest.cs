using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
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
            SetupForumSections();
            SetupForums();
            SetupThreads();
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
                });
            }
        }
        private void SetupForums()
        {
            for (int i = 0; i < NUM_FORUMS; i++)
            {
                int forumSectionId = i % NUM_FORUM_SECTIONS;
                ForumSection section = controllerDb.ForumSections.FindById(forumSectionId);
                Forum forum = new Forum
                {
                    ForumSectionID = forumSectionId,
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
                int forumId = i % NUM_FORUMS;
                Forum forum = controllerDb.Forums.FindById(forumId);
            }
        }

        [TestMethod]
        public void IndexIsNotNull()
        {
            ViewResult result = Controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ThreadsActionWithoutForumIdGivesRedirect()
        {
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task ThreadsValidId()
        {
            // TODO: Create forum, and thread and place in unit of work

            ViewResult result = await Controller.Threads(1) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}

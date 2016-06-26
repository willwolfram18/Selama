using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Areas.Forums.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Tests.Areas.Forum.Controllers
{
    [TestClass]
    public class ForumControllerTest
    {
        [TestMethod]
        public void Index()
        {
            ForumController controller = new ForumController();

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ThreadsNoId()
        {
            ForumController controller = new ForumController();

            RedirectToRouteResult result = controller.Threads() as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ThreadsValidId()
        {
            ForumController controller = new ForumController();

            ViewResult result = controller.Threads(1) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}

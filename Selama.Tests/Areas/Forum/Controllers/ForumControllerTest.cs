using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models.DAL;
using Selama.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Tests.Areas.Forum.Controllers
{
    [TestClass]
    public class ForumControllerTest : _BaseControllerUnitTest<ForumController>
    {
        [TestMethod]
        public void Index()
        {
            ViewResult result = Controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ThreadsNoId()
        {
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task ThreadsValidId()
        {
            // Require dependency injection
            ViewResult result = await Controller.Threads(1) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}

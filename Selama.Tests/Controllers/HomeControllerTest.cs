using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Controllers;
using System.Web.Mvc;

namespace Selama.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : _BaseControllerUnitTest<HomeController>
    {
        [TestMethod]
        public void IndexIsNotNull()
        {
            // Act
            ViewResult result = Controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AboutIsNotNull()
        {
            // Act
            ViewResult result = Controller.About();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void JoinResultIsNotNull()
        {
            // Act
            ViewResult result = Controller.Join();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ErrorResultIsNotNull()
        {
            string errorMessage = "My error message";
            ViewResult result = Controller.Error("My error message") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(errorMessage, result.ViewBag.ErrorMessage);
        }

        [TestMethod]
        public void ErrorResultIsRedirect()
        {
            ActionResult result = Controller.Error() as RedirectToRouteResult;

            Assert.IsNotNull(result);
        }
    }
}

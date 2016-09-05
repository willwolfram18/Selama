using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Controllers;
using System.Web.Mvc;

namespace Selama.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexIsNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AboutIsNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void JoinResultIsNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Join();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ErrorResultIsNotNull()
        {

            HomeController controller = new HomeController();

            string errorMessage = "My error message";
            ViewResult result = controller.Error("My error message") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(errorMessage, result.ViewBag.ErrorMessage);
        }

        [TestMethod]
        public void ErrorResultIsRedirect()
        {
            HomeController controller = new HomeController();

            ActionResult result = controller.Error() as RedirectToRouteResult;

            Assert.IsNotNull(result);
        }
    }
}

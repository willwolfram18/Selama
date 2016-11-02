using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Controllers;
using Selama.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Selama.Tests.Controllers
{
    [TestClass]
    public abstract class _BaseControllerUnitTest<TController>
        where TController : _BaseController, new()
    {
        protected TController Controller { get; private set; }

        protected Mock<ControllerContext> MockControllerContext { get; private set; }

        protected Mock<HttpContextBase> MockHttpContext { get; private set; }

        protected InMemorySession Session { get; private set; }

        protected Mock<IPrincipal> MockUser { get; private set; }

        [TestInitialize]
        public void SetupTest()
        {
            InitializeHttpContext();

            InitializeControllerContext();

            Controller = SetupController();
            Controller.ControllerContext = MockControllerContext.Object;
        }

        #region Overridable setup functions
        protected virtual TController SetupController()
        {
            return new TController();
        }
        protected virtual Mock<ControllerContext> SetupControllerContext()
        {
            return new Mock<ControllerContext>();
        }
        protected virtual Mock<HttpContextBase> SetupHttpContext()
        {
            return new Mock<HttpContextBase>();
        }
        protected virtual InMemorySession SetupSession()
        {
            return new InMemorySession();
        }
        protected virtual Mock<IPrincipal> SetupUser()
        {
            return new Mock<IPrincipal>();
        }
        #endregion

        private void InitializeHttpContext()
        {
            Session = SetupSession();
            MockUser = SetupUser();
            MockHttpContext = SetupHttpContext();
            MockHttpContext.Setup(ctxt => ctxt.Session).Returns(Session);
            MockHttpContext.Setup(ctxt => ctxt.User).Returns(MockUser.Object);
        }

        private void InitializeControllerContext()
        {
            MockControllerContext = SetupControllerContext();
            MockControllerContext.Setup(ctxt => ctxt.HttpContext).Returns(MockHttpContext.Object);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Tests.Controllers
{
    [TestClass]
    public abstract class _BaseControllerUnitTest<TController>
        where TController : _BaseController, new()
    {
        protected TController Controller { get; private set; }

        [TestInitialize]
        public void SetupTest()
        {
            Controller = new TController();
        }
    }
}

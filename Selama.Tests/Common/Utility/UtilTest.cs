using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selama.Tests.Common.Utility
{
    [TestClass]
    public class UtilTest
    {
        const int SEC_IN_MIN = 60;
        const int SEC_IN_HR = SEC_IN_MIN * 60; // 60 sec per min * 60 min per hr
        const int SEC_IN_DAY = SEC_IN_HR * 24;
        const int SEC_IN_WEEK = SEC_IN_DAY * 7;
        const int SEC_IN_MONTH = SEC_IN_DAY * 30;
        const int SEC_IN_YEAR = SEC_IN_DAY * 365;

        [TestMethod]
        public void TestRelativeDateForToday()
        {
            for (int i = 0; i < SEC_IN_MIN - 5; i++)
            {
                Assert.AreEqual<string>(
                    "Less than a minute ago",
                    Util.RelativeDate(DateTime.Now.AddSeconds(i)));
            }

            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddSeconds(-61)) == "1 minute ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-3)) == "3 minutes ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-58)) == "58 minutes ago");

            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-60)) == "1 hour ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-61)) == "1 hour ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-118)) == "1 hour ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-120)) == "2 hours ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddMinutes(-122)) == "2 hours ago");

            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-23)) == "23 hours ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-23).AddMinutes(-58)) == "23 hours ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-24)).StartsWith("Yesterday at "));
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-48).AddMinutes(3)) == "2 days ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-48)) == "2 days ago");
            Assert.IsTrue(Util.RelativeDate(DateTime.Now.AddHours(-53)) == "2 days ago");
        }
    }
}

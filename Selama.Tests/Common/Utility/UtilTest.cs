using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selama.Common.Extensions;
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

        // TODO: Need to determine how to fix this test
        //[TestMethod]
        public void TestRelativeDateForToday()
        {
            for (int i = 0; i < SEC_IN_MIN - 5; i++)
            {
                Assert.AreEqual<string>(
                    "Less than a minute ago",
                    DateTime.Now.AddSeconds(i).ToRelativeDateTimeString()
                );
            }

            Assert.IsTrue(DateTime.Now.AddSeconds(-61).ToRelativeDateTimeString() == "1 minute ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-3).ToRelativeDateTimeString() == "3 minutes ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-58).ToRelativeDateTimeString() == "58 minutes ago");

            Assert.IsTrue(DateTime.Now.AddMinutes(-60).ToRelativeDateTimeString() == "1 hour ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-61).ToRelativeDateTimeString() == "1 hour ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-118).ToRelativeDateTimeString() == "1 hour ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-120).ToRelativeDateTimeString() == "2 hours ago");
            Assert.IsTrue(DateTime.Now.AddMinutes(-122).ToRelativeDateTimeString() == "2 hours ago");

            Assert.IsTrue(DateTime.Now.AddHours(-23).ToRelativeDateTimeString() == "23 hours ago");
            Assert.IsTrue(DateTime.Now.AddHours(-23).AddMinutes(-58).ToRelativeDateTimeString() == "23 hours ago");
            Assert.IsTrue(DateTime.Now.AddHours(-24).ToRelativeDateTimeString().StartsWith("Yesterday at "));
            Assert.IsTrue(DateTime.Now.AddHours(-48).AddMinutes(3).ToRelativeDateTimeString() == "2 days ago");
            Assert.IsTrue(DateTime.Now.AddHours(-48).ToRelativeDateTimeString() == "2 days ago");
            Assert.IsTrue(DateTime.Now.AddHours(-53).ToRelativeDateTimeString() == "2 days ago");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YALV.Core.Domain;
using YALV.Core.Filters.Strings;

namespace YALV.Core.Test
{
    [TestClass]
    public class ContainsAllTokenStringPropertyFilterTest
    {
        [DataTestMethod]
        [DataRow("hallo welt", "A Hallo du Welt da", true)]
        [DataRow("hallo welt", "A welt Hallo du da", false)]
        [DataRow("hallo welt", "hallowelt", true)]
        [DataRow("hallo welt", "welt hallo welt", true)]
        [DataRow("-hallo welt", "hallo welt", false)]
        [DataRow("-hallo welt", "-hallo welt", true)]
        public void Test_IgnoreCaseNoExclusion(string filter, string given, bool expected)
        {
            ContainsAllTokenStringPropertyFilter testee = new ContainsAllTokenStringPropertyFilter(false, true);
            testee.Update(filter);

            LogItem item = new LogItem() { Message = given };

            Assert.IsTrue(testee.Matches(item, LogItemProperty.Message) == expected);
        }

        [DataTestMethod]
        [DataRow("hallo welt", "hallo welt", true)]
        [DataRow("hallo welt", "hallo Welt", false)]
        [DataRow("hallo -welt", "hallo welt", false)]
        [DataRow("hallo welt", "hallo -Welt", false)]
        [DataRow("hallo welt", "hallo -welt", true)]
        public void Test_WithCaseNoExclusion(string filter, string given, bool expected)
        {
            ContainsAllTokenStringPropertyFilter testee = new ContainsAllTokenStringPropertyFilter(false, false);
            testee.Update(filter);

            LogItem item = new LogItem() { Message = given };

            Assert.IsTrue(testee.Matches(item, LogItemProperty.Message) == expected);
        }

        [DataTestMethod]
        [DataRow("hallo welt", "hallo welt", true)]
        [DataRow("hallo welt", "hallo Welt", false)]
        [DataRow("hallo -welt", "hallo velo", true)]
        [DataRow("hallo -welt", "hallo welt", false)]
        [DataRow("hallo -welt", "hallo Welt", true)]
        [DataRow("-welt hallo", "hallo welt", false)]
        [DataRow("-welt hallo", "hallo Welt", true)]
        [DataRow("-hallo -welt", "Hallo Welt", true)]
        public void Test_WithCaseAndExclusion(string filter, string given, bool expected)
        {
            ContainsAllTokenStringPropertyFilter testee = new ContainsAllTokenStringPropertyFilter(true, false);
            testee.Update(filter);

            LogItem item = new LogItem() { Message = given };

            Assert.IsTrue(testee.Matches(item, LogItemProperty.Message) == expected);
        }

        [DataTestMethod]
        [DataRow("hallo welt", "hallo welt", true)]
        [DataRow("hallo Welt", "hallo welt", true)]
        [DataRow("hallo -welt", "hallo velo", true)]
        [DataRow("hallo -welt", "hallo welt", false)]
        [DataRow("hallo -welt", "hallo Welt", false)]
        [DataRow("-welt hallo", "hallo welt", false)]
        [DataRow("-welt hallo", "hallo Welt", false)]
        [DataRow("-hallo -welt", "Hallo Welt", false)]
        public void TestIgnoreCaseWithExclusion(string filter, string given, bool expected)
        {
            ContainsAllTokenStringPropertyFilter testee = new ContainsAllTokenStringPropertyFilter(true, true);
            testee.Update(filter);

            LogItem item = new LogItem() { Message = given };

            Assert.IsTrue(testee.Matches(item, LogItemProperty.Message) == expected);
        }
    }
}

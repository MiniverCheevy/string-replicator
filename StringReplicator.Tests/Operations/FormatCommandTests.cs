using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Operations;

namespace StringReplicator.Tests.Operations
{
    [TestClass()]
    public class FormatCommandTests
    {
        [TestMethod]
        public void Format_TwoStrings_IsOk()
        {
            var csv = string.Format("{0}{1}{0}{1}", "1,2,3", Environment.NewLine);
            var request = new FormatRequest { FormatString = "{0}-{1}-{2}", DataString  = csv};
            var helper = new FormatQuery(request);
            var response = helper.Execute();
            var expected = "1-2-3" + Environment.NewLine + "1-2-3" + Environment.NewLine;
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            Assert.AreEqual(expected, response.Text);
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Operations;
using StringReplicator.Core.Operations.Clean;

namespace StringReplicator.Tests.Operations.Clean
{
    [TestClass()]
    public class SortAndDistinctCommandTests
    {
        [TestMethod()]
        public void SortAndDistinctCommandTest()
        {
            var items = new string[] {"e", "b", "c", "a", "c"};
            var data = string.Join(Environment.NewLine, items);

            var response = new SortAndDistinctCommand(new TextRequest {Text = data}).Execute();
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            var resultData = response.Text.Split(Environment.NewLine.ToCharArray(),
                                                 StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(4, resultData.Count());
            Assert.AreEqual("a", resultData.First());
            Assert.AreEqual("e", resultData.Last());
        }
    }
}
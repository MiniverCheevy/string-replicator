using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Helpers;
using StringReplicator.Core.Operations.History;
using Voodoo.Messages;

namespace StringReplicator.Tests.Operations.History
{
    [TestClass()]
    public class FormatRequestQueryTests
    {

        private TestContext testContextInstance;
       
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        public FormatRequestQueryTests()
        {
            var config = new TestConfig(TestContext);
            Config.RegisterConfig(config);
        }
        [TestMethod()]
        public void FormatRequestQueryTest()
        {
            var response = new LastFormatRequestQuery(new EmptyRequest()).Execute();
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            Assert.AreNotEqual(null, response.Data.DataString);
            Assert.AreNotEqual(null, response.Data.DataString);
        }
    }
}

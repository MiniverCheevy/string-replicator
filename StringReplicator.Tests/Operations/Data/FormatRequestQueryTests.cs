using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Helpers;
using StringReplicator.Core.Operations.Data;
using Voodoo.Messages;

namespace StringReplicator.Tests.Operations.Data
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
            var config = new TestConfig(TestContext.DeploymentDirectory);
            Config.RegisterConfig(config);
        }
        [TestMethod()]
        public void FormatRequestQueryTest()
        {
            var response = new FormatRequestQuery(new EmptyRequest()).Execute();
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            Assert.AreNotEqual(null, response.Data.DataString);
            Assert.AreNotEqual(null, response.Data.DataString);
        }
    }
}

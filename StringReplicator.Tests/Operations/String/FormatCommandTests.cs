using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Helpers;
using StringReplicator.Core.Operations;
using StringReplicator.Core.Operations.Format;

namespace StringReplicator.Tests.Operations.String
{
    [TestClass]
    public class FormatCommandTests
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
        public FormatCommandTests()
        {
            var config = new TestConfig(TestContext);
            Config.RegisterConfig(config);
        }
        
        [TestMethod]
        public void Format_TwoStrings_IsOk()
        {
            var csv = string.Format("{0}{1}{0}{1}", "1,2,3", Environment.NewLine);
            var request = new FormatRequest {FormatString = "{0}-{1}-{2}", DataString = csv};
            var helper = new FormatCommand(request);
            var response = helper.Execute();
            var expected = "1-2-3" + Environment.NewLine + "1-2-3" + Environment.NewLine;
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            Assert.AreEqual(expected, response.Text);
        }

        [TestMethod]
        public void Format_MultiLine_IsOk()
        {

            var data = new string[] { Environment.NewLine,"a,1/1/2012,GreenBlueRed", "b,1/1/2013,BlueYellowGreen", "c,1/1/2014,RedYellowOrange" };

            var csv = string.Format("{1}{0}{2}{0}{3}", data);
            var request = new FormatRequest { FormatString = "Row# {#}:  {0} --> {1:yyyy} --> {2:!}", DataString = csv };
            var helper = new FormatCommand(request);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = helper.Execute();
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedMilliseconds);
            const string expected = @"Row# 0:  a --> 2012 --> Green Blue Red
Row# 1:  b --> 2013 --> Blue Yellow Green
Row# 2:  c --> 2014 --> Red Yellow Orange
";
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            Assert.AreEqual(expected, response.Text);
        }
    }
}
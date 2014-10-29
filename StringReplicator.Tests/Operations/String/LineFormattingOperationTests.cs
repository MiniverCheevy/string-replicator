using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Operations;
using StringReplicator.Core.Operations.Format;
using Voodoo;

namespace StringReplicator.Tests.Operations.String
{
    [TestClass]
    public class FormatHelperTests
    {
        [TestMethod]
        public void Format_TwoStrings_IsOk()
        {
            var request = new LineFormattingRequest {FormatString = "{0},{1}", Arguments = new object[] {"a", "b"}};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute();
            Assert.AreEqual("a,b", response.Text);
        }

        [TestMethod]
        public void Format_TwoStringFirstIsFormattedToFriendlyString_IsOk()
        {
            const string format = "{0:!},{1}";
            var data = new object[] {"RedBlue", "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            Assert.AreEqual("Red Blue,b", response);
        }

        [TestMethod]
        public void Format_TwoStringFirstIsDate_IsOk()
        {
            const string format = "{0:yyyy},{1}";
            var data = new object[] {"1/1/2010".To<DateTime>(), "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            var standardResponse = string.Format(format, data);
            Assert.AreEqual("2010,b", standardResponse);
            Assert.AreEqual("2010,b", response);
        }

        [TestMethod]
        public void Format_TwoStringFirstIsPaddedFormattedInt_IsOk()
        {
            const string format = "{0,12:N0},{1}";
            var data = new object[] {"1504277".To<int>(), "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            var standardResponse = string.Format(format, data);
            Assert.AreEqual("   1,504,277,b", standardResponse);
            Assert.AreEqual("   1,504,277,b", response);
        }

        [TestMethod]
        public void Format_TwoStringFirstIsPaddedFormattedIntBadWhiteSpace_IsOk()
        {
            const string format = "{0, 12:N0},{1}";
            var data = new object[] {"1504277".To<int>(), "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            var standardResponse = string.Format(format, data);
            Assert.AreEqual("   1,504,277,b", standardResponse);
            Assert.AreEqual("   1,504,277,b", response);
        }

        [TestMethod]
        public void Format_TwoStringFirstIsLeftPaddedFormattedInt_IsOk()
        {
            const string format = "{0,-12:N0},{1}";
            var data = new object[] {"1504277".To<int>(), "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            var standardResponse = string.Format(format, data);
            Assert.AreEqual("1,504,277   ,b", standardResponse);
            Assert.AreEqual("1,504,277   ,b", response);
        }

        [TestMethod]
        public void Format_MissingParameter_IsNotOk()
        {
            const string format = "{0}{1}{2}";
            var data = new object[] {"a", "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            Assert.AreEqual(response, Messages.WrongNumberOfArguments);
        }

        [TestMethod]
        public void Format_UnescapedOpenSquiggle_IsNotOk()
        {
            const string format = "public {0} {1} {get;set;";
            var data = new object[] {"a", "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            Assert.AreEqual(response, Messages.FormatError);
        }


        [TestMethod]
        public void Format_UnescapedClosedSquiggle_IsNotOk()
        {
            const string format = "public {0} {1} get;set;}";
            var data = new object[] {"a", "b"};
            var request = new LineFormattingRequest {FormatString = format, Arguments = data};
            var helper = new LineFormattingOperation(request);
            var response = helper.Execute().Text;
            Assert.AreEqual(response, Messages.FormatError);
        }
    }
}
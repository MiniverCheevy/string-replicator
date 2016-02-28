using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Infrastructure;
using StringReplicator.Core.Operations;
using StringReplicator.Core.Operations.Test;
namespace StringReplicator.Tests.Operations.Test
{
    [TestClass()]
    public class TestConnectionQueryTests
    {
        [TestMethod()]
        public void Execute_ValidSql_IsOk()
        {
            var request = new DatabaseRequest
                {
                    DataBaseType = DataBaseType.SqlServer,
                    ServerName = ".",
                    DatabaseName = "master",
                    ConnectionType = ConnectionType.WindowsAuthentication
                };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
        }
        [TestMethod()]
        public void Execute_SqlNoDatabaseName_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                DataBaseType = DataBaseType.SqlServer,
                ServerName = ".",
                ConnectionType = ConnectionType.WindowsAuthentication
            };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(Voodoo.Strings.Validation.validationErrorsOccurred, response.Message);
            Assert.AreEqual(false, response.IsOk);
            Assert.AreEqual("DatabaseName",response.Details.First().Name);
            Assert.AreEqual("required", response.Details.First().Value);
        }
        [TestMethod()]
        public void Execute_SqlNoServerName_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                DataBaseType = DataBaseType.SqlServer,
                DatabaseName = "master",
                ConnectionType = ConnectionType.WindowsAuthentication
            };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(Voodoo.Strings.Validation.validationErrorsOccurred, response.Message);
            Assert.AreEqual(false, response.IsOk);
            Assert.AreEqual("ServerName", response.Details.First().Name);
            Assert.AreEqual("required", response.Details.First().Value);
        }
        [TestMethod()]
        public void Execute_SqlSqlAuthNoUserName_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                ServerName = ".",
                DataBaseType = DataBaseType.SqlServer,
                DatabaseName = "master",
                ConnectionType = ConnectionType.SqlServerAuthentication,
                Password = "foo"
            };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(Voodoo.Strings.Validation.validationErrorsOccurred, response.Message);
            Assert.AreEqual(false, response.IsOk);
            Assert.AreEqual("UserName", response.Details.First().Name);
            Assert.AreEqual("required", response.Details.First().Value);
        }
        [TestMethod()]
        public void Execute_SqlSqlAuthNoPassword_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                ServerName = ".",
                DataBaseType = DataBaseType.SqlServer,
                DatabaseName = "master",
                ConnectionType = ConnectionType.SqlServerAuthentication,
                UserName = "foo"
            };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(Voodoo.Strings.Validation.validationErrorsOccurred, response.Message);
            Assert.AreEqual(false, response.IsOk);
            Assert.AreEqual("Password", response.Details.First().Name);
            Assert.AreEqual("required", response.Details.First().Value);
        }
        [TestMethod()]
        public void Execute_NotSqlNoUdlFile_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                DataBaseType =  DataBaseType.NotSqlServer
            };
            var response = new TestConnectionQuery(request).Execute();
            Assert.AreEqual(Messages.CouldNotFindUdl, response.Message);
            Assert.AreEqual(false, response.IsOk);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Infrastructure;
using StringReplicator.Core.Operations;
using StringReplicator.Core.Operations.Database;

namespace StringReplicator.Tests.Operations.Data
{
    [TestClass()]
    public class DataQueryTests
    {
        [TestMethod()]
        public void Execute_ValidSql_IsOk()
        {
            var request = new DatabaseRequest
                {
                    DataBaseType = DataBaseType.SqlServer,
                    ServerName = ".",
                    DatabaseName = "master",
                    ConnectionType = ConnectionType.WindowsAuthentication,
                    Sql = "SELECT TOP 1 'Smith, John',[TABLE_CATALOG],[TABLE_SCHEMA] FROM [INFORMATION_SCHEMA].[TABLES]"
                };
            var response = new DataQuery(request).Execute();
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(true, response.IsOk);
            
            const string expected = "\"Smith, John\",master,dbo";
            //TODO: write extension method to compare strings
            //Debug.WriteLine(expected);
            //Debug.WriteLine(response.Text);
            //Assert.AreEqual(expected, response.Text);
            //Assert.AreEqual(expected.Length, response.Text.Length);

        }
        [TestMethod()]
        public void Execute_InvalidValidSql_IsNotOk()
        {
            var request = new DatabaseRequest
            {
                DataBaseType = DataBaseType.SqlServer,
                ServerName = ".",
                DatabaseName = "master",
                ConnectionType = ConnectionType.WindowsAuthentication,
                Sql = "SELECT TOP x 'Smith, John',[TABLE_CATALOG],[TABLE_SCHEMA] FROM [INFORMATION_SCHEMA].[TABLES]"
            };
            var response = new DataQuery(request).Execute();
            Assert.AreEqual("Incorrect syntax near 'x'.", response.Message);
            Assert.AreEqual(false, response.IsOk);            

        }
    }
}
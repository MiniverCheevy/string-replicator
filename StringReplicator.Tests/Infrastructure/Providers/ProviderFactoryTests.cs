using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Helpers;
using StringReplicator.Core.Infrastructure;
using StringReplicator.Core.Infrastructure.Providers;
using StringReplicator.Core.Operations;

namespace StringReplicator.Tests.Infrastructure.Providers
{
    [TestClass()]
    public class ProviderFactoryTests
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
        public ProviderFactoryTests()
        {
            var config = new TestConfig(TestContext);
            Config.RegisterConfig(config);
        }

        [TestMethod()]
        public void GetSqlProviderTest()
        {
            var request = new DatabaseRequest
            {
                
                DataBaseType = DataBaseType.SqlServer,
                ConnectionType = ConnectionType.WindowsAuthentication,
                DatabaseName = "master",
                ServerName = "."
            };
            var provider = ProviderFactory.GetProvider(request);
            using (var connection = provider.GetConnection())
            {
                using (var command = provider.GetUnsafeCommand(connection, "Select * from sys.databases"))
                {
                    
                    var ds = new DataSet();
                    provider.GetDataAdapter(command).Fill(ds);
                    Assert.AreEqual(1, ds.Tables.Count);
                    var dt = ds.Tables[0];
                    Assert.AreNotEqual(0, dt.Rows.Count);
                }
            }
        }

        [TestMethod()]
        public void GetNotSqlProvider_FunkyPath_Test()
        {
            var request = new DatabaseRequest
                {
                    UdlFile = Path.Combine(Config.Current.GetRootPath(), @"Data\Path With Spaces", "test.udl"),
                    DataBaseType = DataBaseType.NotSqlServer
                };
            invokeNotSqlProvider(request);
        }
        [TestMethod()]
        public void GetNotSqlProvider_NormalPath_Test()
        {
            var request = new DatabaseRequest
            {
                UdlFile = Config.Current.GetUdlPath(),
                DataBaseType = DataBaseType.NotSqlServer
            };
            invokeNotSqlProvider(request);
        }

        private static void invokeNotSqlProvider(DatabaseRequest request)
        {
            var provider = ProviderFactory.GetProvider(request);
            using (var connection = provider.GetConnection())
            {
                using (var command = provider.GetUnsafeCommand(connection, "Select * from sys.databases"))
                {
                    var ds = new DataSet();
                    provider.GetDataAdapter(command).Fill(ds);
                    Assert.AreEqual(1, ds.Tables.Count);
                    var dt = ds.Tables[0];
                    Assert.AreNotEqual(0, dt.Rows.Count);
                }
            }
        }
    }
}

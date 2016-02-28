using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using StringReplicator.Core.Operations;

namespace StringReplicator.Core.Infrastructure.Providers
{
    public class ProviderFactory
    {
        public static IDbHelper GetProvider(DatabaseRequest request)
        {
            switch (request.DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return buildSqlServerProvider(request);
                case DataBaseType.NotSqlServer:
                    return buildNotSqlServerProvider(request);
                default:
                    throw new ArgumentOutOfRangeException("Unknown DataBaseType");
            }
        }

        private static IDbHelper buildNotSqlServerProvider(DatabaseRequest request)
        {
            var connectionString = string.Format("File name = {0}", request.UdlFile);
            return new NotSqlServerDbHelper(connectionString);
        }

        private static IDbHelper buildSqlServerProvider(DatabaseRequest request)
        {
            string connectionString = null;


            var builder = new SqlConnectionStringBuilder
                {
                    ApplicationName = "String Replicator",
                    DataSource = request.ServerName,
                    ConnectTimeout = 3,
                    InitialCatalog = request.DatabaseName,
                    IntegratedSecurity = request.ConnectionType == ConnectionType.WindowsAuthentication
                };
            if (request.ConnectionType != ConnectionType.WindowsAuthentication)
            {
                builder.UserID = request.UserName;
                builder.Password = request.Password;
            }
            connectionString = builder.ConnectionString;
            return new SqlServerDbHelper(connectionString);
        }
    }
}
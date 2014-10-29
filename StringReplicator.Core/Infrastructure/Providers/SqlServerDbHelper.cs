using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StringReplicator.Core.Infrastructure.Providers
{
    public class SqlServerDbHelper : DbHelper<SqlConnection, SqlDataAdapter>
    {
        private string connectionString;

        public SqlServerDbHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override string ConnectionString
        {
            get { return connectionString; }
        }

        public override IDataAdapter GetDataAdapter(IDbCommand command)
        {
            var sqlCommand = command as SqlCommand;
            return new SqlDataAdapter(sqlCommand);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace StringReplicator.Core.Infrastructure.Providers
{
    public class NotSqlServerDbHelper: DbHelper<OleDbConnection, OleDbDataAdapter>
    {
       private string connectionString;

        public NotSqlServerDbHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override string ConnectionString
        {
            get { return connectionString; }
        }

        public override IDataAdapter GetDataAdapter(IDbCommand command)
        {
            var oleDbCommand = command as OleDbCommand;
            return new OleDbDataAdapter(oleDbCommand);
        }
    }
}

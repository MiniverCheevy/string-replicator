using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StringReplicator.Core.Infrastructure.Providers
{
    public abstract class DbHelper<TDbConnection, TDataAdapter> : IDbHelper
        where TDbConnection : IDbConnection, new()
        where TDataAdapter : IDataAdapter, new()
    {
        public abstract string ConnectionString { get; }

        public IDbConnection GetConnection()
        {
            var connection = new TDbConnection() {ConnectionString = ConnectionString};
            return connection;
        }

        public IDbCommand GetUnsafeCommand(IDbConnection connection, string procedureName)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = int.MaxValue;
            command.CommandText = procedureName;
            command.CommandType = CommandType.Text;
            return command;
        }

        public abstract IDataAdapter GetDataAdapter(IDbCommand command);
        
    }
}
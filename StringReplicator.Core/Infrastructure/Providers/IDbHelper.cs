using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StringReplicator.Core.Infrastructure.Providers
{
    public interface IDbHelper
    {
        string ConnectionString { get; }
        IDbConnection GetConnection();        
        IDbCommand GetUnsafeCommand(IDbConnection connection, string procedureName);
        IDataAdapter GetDataAdapter(IDbCommand command);
    }
}
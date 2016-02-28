using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StringReplicator.Core.CodeGeneration;
using StringReplicator.Core.Infrastructure;
using StringReplicator.Core.Infrastructure.Providers;
using Voodoo.Infrastructure;
using Voodoo.Messages;
using Voodoo.Operations;
using Voodoo;

namespace StringReplicator.Core.Operations.Test
{    
    
    public abstract class DatabaseQuery<TResponse> : Query<DatabaseRequest, TResponse>
        where TResponse: class, IResponse, new()
    {
        protected DatabaseQuery(DatabaseRequest request) : base(request)
        {
        }

        protected override void Validate()
        {
            base.Validate();
            switch (request.DataBaseType)
            {
                case DataBaseType.SqlServer:
                    validateSqlServer();
                    break;
                case DataBaseType.NotSqlServer:
                    validateNotSqlServer();
                    break;
            }
        }

        private void validateNotSqlServer()
        {
            if (string.IsNullOrWhiteSpace(request.UdlFile) || ! File.Exists(request.UdlFile))
                throw new LogicException(Messages.CouldNotFindUdl);
        }


        private void validateSqlServer()
        {
            var details = new List<INameValuePair>();
            if (string.IsNullOrWhiteSpace(request.ServerName))
                details.Add("ServerName", "required");
            if (string.IsNullOrWhiteSpace(request.DatabaseName))
                details.Add("DatabaseName", "required");

            if (request.ConnectionType == ConnectionType.SqlServerAuthentication)
            {
                if (string.IsNullOrWhiteSpace(request.UserName))
                    details.Add("UserName", "required");
                if (string.IsNullOrWhiteSpace(request.Password))
                    details.Add("Password", "required");
            }
            if (details.Any())
                throw new LogicException(Voodoo.Strings.Validation.validationErrorsOccurred) {Details = details};
        }        
    }
}

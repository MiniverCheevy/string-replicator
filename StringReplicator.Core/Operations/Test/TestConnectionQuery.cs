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
using Message = Voodoo.Validation.Infrastructure.Messages;
using Voodoo;

namespace StringReplicator.Core.Operations.Test
{
    [Rest(Verb.Get, Resources.Test)]
    public class TestConnectionQuery : DatabaseQuery<Response>
    {
        public TestConnectionQuery(DatabaseRequest request) : base(request)
        {
        }
        protected override Response ProcessRequest()
        {
            var provider = ProviderFactory.GetProvider(request);
            using (var connection = provider.GetConnection())
            {
                connection.Open();
            }
            return response;
        }
    }
}
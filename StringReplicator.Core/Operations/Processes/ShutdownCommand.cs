using System;
using System.Collections.Generic;
using System.Linq;
using StringReplicator.Core.CodeGeneration;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Processes
{
    [Rest(Verb.Delete, Resources.Session)]
    public class ShutdownCommand : Command<EmptyRequest, Response>
    {
        public ShutdownCommand(EmptyRequest request)
            : base(request)
        {
        }

        protected override Response ProcessRequest()
        {
            Environment.Exit(0);
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringReplicator.Core.CodeGeneration;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Format
{
    [Rest(Verb.Post, Resources.String)]
    public class ShutdownCommand : Command<FormatRequest, Response>
    {
        public ShutdownCommand(FormatRequest request)
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

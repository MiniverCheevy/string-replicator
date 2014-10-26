using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using StringReplicator.Core.Operations.Processes;
using Voodoo;
using Voodoo.Messages;

namespace StringReplicator.Core.Controllers
{
    public class SessionController : ApiController
    {
        [HttpDelete]
        public Response Delete([FromUri] EmptyRequest request)
        {
            var op = new ShutdownCommand(request);
            var response = op.Execute();
            return response;
        }
    }
}
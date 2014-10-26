using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using StringReplicator.Core.Operations;
using Voodoo;
using Voodoo.Messages;

namespace StringReplicator.Core.Controllers
{
    public class StringController : ApiController
    {
        [HttpGet]
        public TextResponse Get([FromUri] FormatRequest request)
        {
            var op = new FormatQuery(request);
            var response = op.Execute();
            return response;
        }
    }
}

//***************************************************************
//This code just called you a tool
//What I meant to say is that this code was generated by a tool
//so don't mess with it unless you're debugging
//subject to change without notice, might regenerate while you're reading, etc
//***************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Voodoo;
namespace StringReplicator.Core.Controllers
{
	public class CleanController : ApiController
	{
		
		[HttpPost]
		public Voodoo.Messages.TextResponse Post
			([FromBody] StringReplicator.Core.Operations.TextRequest request)
			{
				var op = new StringReplicator.Core.Operations.Clean.SortAndDistinctCommand(request);
				var response = op.Execute();
				return response;
			}

	}
}

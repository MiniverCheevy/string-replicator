
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
	public class TestController : ApiController
	{
		
		[HttpGet]
		public Voodoo.Messages.Response Get
			([FromUri] StringReplicator.Core.Operations.DatabaseRequest request)
			{
				var op = new StringReplicator.Core.Operations.Test.TestConnectionQuery(request);
				var response = op.Execute();
				return response;
			}

	}
}

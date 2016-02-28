using System;
using System.Collections.Generic;
using System.Linq;

namespace StringReplicator.Core.CodeGeneration
{
    public enum Verb
    {
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3
    }

    public class RestAttribute : Attribute
    {
        public RestAttribute(Verb verb, string resource)
        {
            this.Verb = verb;
            this.Resource = resource;
        }

        public Verb Verb { get; set; }

        public string Resource { get; set; }
    }
}
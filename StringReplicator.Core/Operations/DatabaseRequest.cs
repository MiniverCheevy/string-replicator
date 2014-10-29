using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StringReplicator.Core.Infrastructure;
using Voodoo;
using Voodoo.Messages;

namespace StringReplicator.Core.Operations
{
    public class DatabaseRequest
    {
        [Required]
        public DataBaseType DataBaseType { get; set; }
        [Required]
        public ConnectionType ConnectionType { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string UdlFile { get; set; }
        public string Sql { get; set; }

        public NameValuePair[] ConnectionTypes {
            get { return typeof (ConnectionType).ToINameValuePairList().Select(c=> new NameValuePair(c.Name, c.Value)).ToArray(); }
            set { ; }
        }
    }
}

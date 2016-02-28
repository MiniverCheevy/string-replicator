using System;
using System.Collections.Generic;
using System.Linq;

namespace StringReplicator.Core.Operations.Format
{
    public class LineFormattingRequest
    {
        public string FormatString { get; set; }
        public Object[] Arguments { get; set; }
    }
}
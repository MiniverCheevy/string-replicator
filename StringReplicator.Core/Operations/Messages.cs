using System;
using System.Collections.Generic;
using System.Linq;

namespace StringReplicator.Core.Operations
{
    public class Messages
    {
        public const string CouldNotFindUdl = "Could not find Udl file.";

        public const string FormatError =
            "Input string was not in a correct format. Or put another way you probably have some unescaped { or } characters, you can fix this by doubling them up {{ or }}";

        public const string WrongNumberOfArguments =
            "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.  Or put another way you don't have enough arguments to cover all of your formats.";
    }
}
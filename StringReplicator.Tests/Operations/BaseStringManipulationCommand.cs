using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Tests.Operations
{
    public abstract class BaseStringManipulationCommand: Command<TextRequest, TextResponse>
    {
        protected BaseStringManipulationCommand(TextRequest request) : base(request)
        {
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using System;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations
{
    public abstract class BaseStringManipulationCommand : Command<TextRequest, TextResponse>
    {
        protected BaseStringManipulationCommand(TextRequest request) : base(request)
        {
        }
    }
}
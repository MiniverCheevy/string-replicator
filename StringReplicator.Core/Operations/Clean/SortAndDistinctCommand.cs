using System;
using System.Collections.Generic;
using System.Linq;
using StringReplicator.Core.CodeGeneration;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Clean
{
    [Rest(Verb.Post, Resources.Clean)]
    public class SortAndDistinctCommand:Command<TextRequest,TextResponse>
    {
        public SortAndDistinctCommand(TextRequest request) : base(request)
        {
        }

        protected override TextResponse ProcessRequest()
        {
            var data = request.Text
                .To<string>()
                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .OrderBy(c=>c)
                .ToArray();

            response.Text = String.Join(Environment.NewLine, data)
                                    .TrimStart(Environment.NewLine.ToCharArray());
            return response;
        }
    }
}

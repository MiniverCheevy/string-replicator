using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringReplicator.Core.CodeGeneration;
using StringReplicator.Core.Helpers;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Database
{
    [Rest(Verb.Put, Resources.Database)]
    public class CreateUdlFileCommand: Command<EmptyRequest,TextResponse>
    {
        public CreateUdlFileCommand(EmptyRequest request) : base(request)
        {
        }

        protected override TextResponse ProcessRequest() 
        {
            var path = Config.Current.GetUdlPath();
            IoNic.KillFile(path);
            IoNic.WriteFile(string.Empty, path);
            IoNic.ShellExecute(path);
            response.Text = path;
            return response;
        }
    }
}

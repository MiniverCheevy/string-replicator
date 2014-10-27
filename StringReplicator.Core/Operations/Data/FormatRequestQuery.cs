using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StringReplicator.Core.CodeGeneration;
using StringReplicator.Core.Helpers;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Data
{
    [Rest(Verb.Get, Resources.Data)]
    public class FormatRequestQuery: Query<EmptyRequest, Response<FormatRequest>>
    {
        public FormatRequestQuery(EmptyRequest request) : base(request)
        {
        }

        protected override Response<FormatRequest> ProcessRequest()
        {
            var path = Config.Current.CurrentFilePath();
            if (File.Exists(path))
            {
                var json = IoNic.ReadFile(path);
                response.Data = JsonConvert.DeserializeObject<FormatRequest>(json);
            }
            return response;
        }
    }
}

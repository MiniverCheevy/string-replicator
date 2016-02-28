using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using StringReplicator.Core.CodeGeneration;
using StringReplicator.Core.Helpers;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.History
{
    [Rest(Verb.Get, Resources.History)]
    public class LastFormatRequestQuery: Query<EmptyRequest, Response<FormatRequest>>
    {
        public LastFormatRequestQuery(EmptyRequest request) : base(request)
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

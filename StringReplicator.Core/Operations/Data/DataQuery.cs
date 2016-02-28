using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringReplicator.Core.Infrastructure.Providers;
using StringReplicator.Core.Operations.Test;
using Voodoo;
using Voodoo.Messages;

namespace StringReplicator.Core.Operations.Data
{
    public class DataQuery:DatabaseQuery<TextResponse>
    {
        public DataQuery(DatabaseRequest request) : base(request)
        {
        }
        protected override void Validate()
        {
            
        }
        protected override TextResponse ProcessRequest()
        {
            const string quoteFormat = "\"{0}\"";
            const string comma = ",";
            var newLine = Environment.NewLine;
            var builder = new StringBuilder();
            var provider = ProviderFactory.GetProvider(request);
            response.NumberOfRowsEffected = 0;
            
            using (var connection = provider.GetConnection())
            {
                connection.Open();
                using (var command = provider.GetUnsafeCommand(connection, request.Sql))
                {
                    var reader = command.ExecuteReader();
                    var columns = reader.FieldCount;
                    while (reader.Read())
                    {
                        for (var i = 0; i < columns; i ++)
                        {
                            var value = reader[i].To<string>()
                                                 .Replace("{", "{{")
                                                 .Replace("}", "}}");                        
                            if (value.Contains(","))
                                value = string.Format(quoteFormat, value);

                            builder.Append(value);
                            if (i!=columns-1)
                                builder.Append(comma);
                        }
                        builder.Append(newLine);
                        response.NumberOfRowsEffected += 1;
                    }
                }            
            }
            response.Text = builder.ToString();
            return response;
        }
    }
}

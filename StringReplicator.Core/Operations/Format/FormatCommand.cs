using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using StringReplicator.Core.CodeGeneration;
using StringReplicator.Core.Helpers;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Format
{
    [Rest(Verb.Post, Resources.String)]
    public class FormatCommand : Command<FormatRequest, TextResponse>
    {
        private List<string[]> csvData;
        private StringBuilder output = new StringBuilder();

        public FormatCommand(FormatRequest request) : base(request)
        {
        }

        protected override TextResponse ProcessRequest()
        {
            saveRequest();
            buildCsv();
            buildOutput();
            response.Text = output.ToString();

            return response;
        }

        private void saveRequest()
        {
            var path = Config.Current.CurrentFilePath();
            var directory = Path.GetDirectoryName(path);
            IoNic.MakeDir(directory);
            var json = JsonConvert.SerializeObject(request);
            IoNic.WriteFile(json, path);
        }

        private void buildCsv()
        {
            csvData = new List<string[]>();
            using (var sr = new StringReader(request.DataString))
            using (var reader = new CsvReader(sr, new CsvConfiguration {HasHeaderRecord = false,}))
            {
                while (reader.Read())
                {
                    csvData.Add(reader.CurrentRecord);
                }
            }
        }

        private void buildOutput()
        {
            var rowNumber = 0;
            foreach (var row in csvData)
            {
                var tempFormat = request.FormatString;

                tempFormat = tempFormat.Replace("{#}", rowNumber.ToString());
                tempFormat = tempFormat.Replace("{+}", (rowNumber + 1).ToString());

                var helper =
                    new LineFormattingOperation(new LineFormattingRequest
                    {
                        FormatString = tempFormat,
                        Arguments = 
                            row.Select(t => t.To<string>().Trim())
                                .Cast<object>()
                                .ToArray()
                    });
                var formattingResponse = helper.Execute();
                output.Append(formattingResponse.Text);
                output.Append(Environment.NewLine);
                rowNumber += 1;
            }
        }
    }
}
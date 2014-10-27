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
    public class FormatQuery : Command<FormatRequest, TextResponse>
    {
        private List<string[]> csvData;
        private StringBuilder output = new StringBuilder();

        public FormatQuery(FormatRequest request) : base(request)
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
                var inputRow = new List<object>();

                var tempFormat = request.FormatString;

                tempFormat = tempFormat.Replace("{#}", rowNumber.ToString());
                tempFormat = tempFormat.Replace("{+}", (rowNumber + 1).ToString());

                for (var i = 0; i < row.Length; i++)
                {
                    inputRow.Add(row[i].To<string>().Trim());
                }
                var helper =
                    new LineFormattingOperation(new LineFormattingRequest
                    {
                        FormatString = tempFormat,
                        Arguments = inputRow.ToArray()
                    });
                var formattingResponse = helper.Execute();
                output.Append(formattingResponse.Text);
                output.Append(Environment.NewLine);
                rowNumber += 1;
            }
        }
    }
}
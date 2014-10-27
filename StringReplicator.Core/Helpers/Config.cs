using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringReplicator.Core.Helpers
{
    public class Config
    {
        public static string GetRootPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public static string CurrentFilePath()
        {
            return Path.Combine(Config.GetRootPath(), "Data", "current.json");
        }
    }
}

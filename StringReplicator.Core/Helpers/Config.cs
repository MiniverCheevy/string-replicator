using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringReplicator.Core.Helpers
{
    public class Config : IConfig
    {
        private static IConfig current = null;

        public static IConfig Current {
            get { return current ?? new Config(); }
        }

        public static void RegisterConfig(IConfig config)
        {
            current = config;
        }

        public virtual string GetRootPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public string CurrentFilePath()
        {
            return Path.Combine(GetRootPath(), "Data", "current.json");
        }
    }
}

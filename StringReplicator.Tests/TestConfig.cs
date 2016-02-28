using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReplicator.Core.Helpers;

namespace StringReplicator.Tests
{
    public class TestConfig: Config
    {
        private string path;

        public TestConfig(TestContext testContext)

        {
            if (testContext != null)
                this.path = testContext.DeploymentDirectory;
            else
                path = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
        }
        public override string GetRootPath()
        {
            return path;
        }
    }
}
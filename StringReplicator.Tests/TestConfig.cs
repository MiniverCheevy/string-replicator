using StringReplicator.Core.Helpers;

namespace StringReplicator.Tests
{
    public class TestConfig: Config
    {
        private string path;

        public TestConfig(string path)
        {
            this.path = path;
        }
        public override string GetRootPath()
        {
            return path;
        }
    }
}
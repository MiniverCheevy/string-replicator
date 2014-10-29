namespace StringReplicator.Core.Helpers
{
    public interface IConfig
    {
        string GetRootPath();
        string CurrentFilePath();
        string GetUdlPath();
    }
}
namespace OrderImporter.Common.Log
{
    public interface ILog
    {
        void Error(string message);
        void Info(string message);
    }
}
namespace OrderImporter.Common.Log
{
    public class ConsoleLog : ILog
    {
        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        private void Log(LogLevel level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logLevelString = level.ToString().ToUpper();

            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = level == LogLevel.Error ? ConsoleColor.Red : ConsoleColor.Green;

            Console.WriteLine($"[{timestamp}] [{logLevelString}] {message}");

            Console.ForegroundColor = originalColor;
        }
    }
}

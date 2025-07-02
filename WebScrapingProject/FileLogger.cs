namespace WebScrapingProject
{
    public class FileLogger : ILogger
    {
        private readonly string _logPath;

        public FileLogger()
        {
            var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDir);
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _logPath = Path.Combine(logsDir, $"log-{timestamp}.txt");
        }

        public void LogError(string message)
        {
            try
            {
                File.AppendAllText(_logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}\n");
            }
            catch { }
        }
    }
}
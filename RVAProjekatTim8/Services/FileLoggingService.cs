using RVAProjekatTim8.Interfaces;
using System;
using System.IO;

namespace RVAProjekatTim8.Services
{
    public class FileLoggingService : ILoggingService
    {
        private readonly string _logFilePath;
        private readonly object _writeLock = new();

        public FileLoggingService(string logFilePath)
        {
            _logFilePath = logFilePath;

            var directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void LogActivity(string message)
        {
            var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            // Lock zbog potencijalnih konkurentnih poziva (npr. simulacija stanja
            // metrike koja radi na pozadinskoj niti, paralelno sa UI akcijama).
            lock (_writeLock)
            {
                File.AppendAllLines(_logFilePath, new[] { logLine });
            }
        }
    }
}

using System.IO;

namespace SmartoLifeCRM.App.Services;

public sealed class FileLoggingService : ILoggingService
{
    private readonly IAppPathService _pathService;

    public FileLoggingService(IAppPathService pathService)
    {
        _pathService = pathService;
    }

    public void Info(string message)
    {
        Write("INFO", message);
    }

    public void Error(Exception exception, string message)
    {
        Write("ERROR", $"{message}{Environment.NewLine}{exception}");
    }

    private void Write(string level, string message)
    {
        Directory.CreateDirectory(_pathService.LogsDirectory);
        var logPath = Path.Combine(_pathService.LogsDirectory, $"smartolife-{DateTimeOffset.UtcNow:yyyyMMdd}.log");
        var line = $"{DateTimeOffset.UtcNow:O} [{level}] {message}{Environment.NewLine}";
        File.AppendAllText(logPath, line);
    }
}


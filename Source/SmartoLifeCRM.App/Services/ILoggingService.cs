namespace SmartoLifeCRM.App.Services;

public interface ILoggingService
{
    void Info(string message);

    void Error(Exception exception, string message);
}


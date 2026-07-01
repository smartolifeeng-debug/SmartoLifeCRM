namespace SmartoLifeCRM.App.Services;

public interface IErrorHandler
{
    void Handle(Exception exception, string userMessage);
}


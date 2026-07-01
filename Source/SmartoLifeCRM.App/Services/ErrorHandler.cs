using System.Windows;

namespace SmartoLifeCRM.App.Services;

public sealed class ErrorHandler : IErrorHandler
{
    private readonly ILoggingService _loggingService;

    public ErrorHandler(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public void Handle(Exception exception, string userMessage)
    {
        _loggingService.Error(exception, userMessage);

        MessageBox.Show(
            userMessage,
            "Smarto Life CRM",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}


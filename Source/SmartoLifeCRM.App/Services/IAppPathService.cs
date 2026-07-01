namespace SmartoLifeCRM.App.Services;

public interface IAppPathService
{
    string AppDataDirectory { get; }

    string DatabasePath { get; }

    string LogsDirectory { get; }

    string SettingsDirectory { get; }
}


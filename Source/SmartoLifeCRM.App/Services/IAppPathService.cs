namespace SmartoLifeCRM.App.Services;

public interface IAppPathService
{
    string AppDataDirectory { get; }

    string DatabasePath { get; }
}


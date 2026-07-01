namespace SmartoLifeCRM.App.Services;

public interface IConfigurationService
{
    string ApplicationName { get; }

    string Version { get; }

    string EnvironmentName { get; }
}


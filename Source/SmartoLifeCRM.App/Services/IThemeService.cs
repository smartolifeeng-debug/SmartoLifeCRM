namespace SmartoLifeCRM.App.Services;

public interface IThemeService
{
    string CurrentThemeName { get; }

    string PrimaryColorName { get; }
}


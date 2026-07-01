using System.IO;

namespace SmartoLifeCRM.App.Services;

public sealed class AppPathService : IAppPathService
{
    public string AppDataDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SmartoLifeCRM");

    public string DatabasePath => Path.Combine(AppDataDirectory, "SmartoLifeCRM.db");
}


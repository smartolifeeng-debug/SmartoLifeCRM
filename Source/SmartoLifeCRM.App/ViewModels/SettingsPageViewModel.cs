using System.Collections.ObjectModel;

namespace SmartoLifeCRM.App.ViewModels;

public sealed class SettingsPageViewModel : PageViewModelBase
{
    public SettingsPageViewModel()
        : base("Settings", "Settings", "Shell placeholders for application configuration.", "\uE713")
    {
        Sections = new ObservableCollection<ModuleSectionViewModel>
        {
            new("Theme", "Smarto Fluent Light theme and orange branding.", "Placeholder"),
            new("Company Profile", "Central Smarto Life company identity settings.", "Placeholder"),
            new("Database", "Local SQLite database initialization status.", "Placeholder"),
            new("Backup", "Future backup settings placeholder only.", "Not implemented"),
            new("Application Information", "Version, environment, and shell information.", "v0.3.0")
        };
    }

    public ObservableCollection<ModuleSectionViewModel> Sections { get; }
}


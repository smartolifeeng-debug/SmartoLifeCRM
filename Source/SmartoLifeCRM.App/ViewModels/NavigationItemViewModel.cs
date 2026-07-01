namespace SmartoLifeCRM.App.ViewModels;

public sealed class NavigationItemViewModel : ObservableObject
{
    private bool _isActive;

    public NavigationItemViewModel(string moduleKey, string title, string description, string iconGlyph)
    {
        ModuleKey = moduleKey;
        Title = title;
        Description = description;
        IconGlyph = iconGlyph;
    }

    public string ModuleKey { get; }

    public string Title { get; }

    public string Description { get; }

    public string IconGlyph { get; }

    public bool IsEnabled => true;

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
}


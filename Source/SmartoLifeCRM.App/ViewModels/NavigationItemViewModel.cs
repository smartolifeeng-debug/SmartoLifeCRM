namespace SmartoLifeCRM.App.ViewModels;

public sealed class NavigationItemViewModel : ObservableObject
{
    private bool _isActive;

    public NavigationItemViewModel(string title, string description, string iconGlyph, bool isEnabled)
    {
        Title = title;
        Description = description;
        IconGlyph = iconGlyph;
        IsEnabled = isEnabled;
    }

    public string Title { get; }

    public string Description { get; }

    public string IconGlyph { get; }

    public bool IsEnabled { get; }

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
}


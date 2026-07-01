namespace SmartoLifeCRM.App.ViewModels;

public sealed class QuickActionViewModel
{
    public QuickActionViewModel(string title, string iconGlyph, string description)
    {
        Title = title;
        IconGlyph = iconGlyph;
        Description = description;
    }

    public string Title { get; }

    public string IconGlyph { get; }

    public string Description { get; }
}


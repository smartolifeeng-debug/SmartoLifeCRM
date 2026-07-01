namespace SmartoLifeCRM.App.ViewModels;

public abstract class PageViewModelBase : ObservableObject
{
    protected PageViewModelBase(string moduleKey, string title, string subtitle, string iconGlyph)
    {
        ModuleKey = moduleKey;
        Title = title;
        Subtitle = subtitle;
        IconGlyph = iconGlyph;
    }

    public string ModuleKey { get; }

    public string Title { get; }

    public string Subtitle { get; }

    public string IconGlyph { get; }
}

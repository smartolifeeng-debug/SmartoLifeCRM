namespace SmartoLifeCRM.App.ViewModels;

public sealed class DashboardMetricViewModel
{
    public DashboardMetricViewModel(string title, string value, string subtitle, string iconGlyph)
    {
        Title = title;
        Value = value;
        Subtitle = subtitle;
        IconGlyph = iconGlyph;
    }

    public string Title { get; }

    public string Value { get; }

    public string Subtitle { get; }

    public string IconGlyph { get; }
}


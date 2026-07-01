namespace SmartoLifeCRM.App.ViewModels;

public sealed class RecentRecordViewModel
{
    public RecentRecordViewModel(string title, string subtitle, string status)
    {
        Title = title;
        Subtitle = subtitle;
        Status = status;
    }

    public string Title { get; }

    public string Subtitle { get; }

    public string Status { get; }
}


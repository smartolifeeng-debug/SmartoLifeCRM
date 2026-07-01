namespace SmartoLifeCRM.App.ViewModels;

public sealed class ActivityItemViewModel
{
    public ActivityItemViewModel(string title, string description, string timeLabel)
    {
        Title = title;
        Description = description;
        TimeLabel = timeLabel;
    }

    public string Title { get; }

    public string Description { get; }

    public string TimeLabel { get; }
}


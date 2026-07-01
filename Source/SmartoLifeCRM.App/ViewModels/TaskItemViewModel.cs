namespace SmartoLifeCRM.App.ViewModels;

public sealed class TaskItemViewModel
{
    public TaskItemViewModel(string title, string dueLabel, string status)
    {
        Title = title;
        DueLabel = dueLabel;
        Status = status;
    }

    public string Title { get; }

    public string DueLabel { get; }

    public string Status { get; }
}


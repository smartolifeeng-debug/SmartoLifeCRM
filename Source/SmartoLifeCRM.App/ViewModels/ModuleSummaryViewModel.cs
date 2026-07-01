namespace SmartoLifeCRM.App.ViewModels;

public sealed class ModuleSummaryViewModel
{
    public ModuleSummaryViewModel(string title, string description, string status)
    {
        Title = title;
        Description = description;
        Status = status;
    }

    public string Title { get; }

    public string Description { get; }

    public string Status { get; }
}


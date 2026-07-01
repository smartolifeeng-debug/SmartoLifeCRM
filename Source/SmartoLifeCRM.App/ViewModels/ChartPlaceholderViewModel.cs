namespace SmartoLifeCRM.App.ViewModels;

public sealed class ChartPlaceholderViewModel
{
    public ChartPlaceholderViewModel(string title, string description, string valueLabel)
    {
        Title = title;
        Description = description;
        ValueLabel = valueLabel;
    }

    public string Title { get; }

    public string Description { get; }

    public string ValueLabel { get; }
}


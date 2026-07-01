using SmartoLifeCRM.App.ViewModels;

namespace SmartoLifeCRM.App.Services;

public interface INavigationService
{
    PageViewModelBase CurrentPage { get; }

    event EventHandler<PageViewModelBase>? CurrentPageChanged;

    void NavigateTo(string moduleKey);
}


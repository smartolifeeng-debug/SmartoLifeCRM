using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using SmartoLifeCRM.App.Services;

namespace SmartoLifeCRM.App.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private bool _isSidebarExpanded = true;
    private PageViewModelBase _currentPage;
    private ModulePageViewModel? _currentModulePage;
    private string _globalSearchText = string.Empty;

    public MainViewModel(
        INavigationService navigationService,
        IConfigurationService configurationService,
        IThemeService themeService)
    {
        _navigationService = navigationService;
        _currentPage = navigationService.CurrentPage;
        _currentModulePage = CurrentPage as ModulePageViewModel;

        NavigationItems = new ObservableCollection<NavigationItemViewModel>
        {
            new("Dashboard", "Dashboard", "Executive overview", "\uE80F"),
            new("Contacts", "Contacts", "Unified CRM records", "\uE77B"),
            new("Products", "Products", "Catalog workspace", "\uE7BF"),
            new("Quotations", "Quotations", "Quotation workspace", "\uE8A5"),
            new("Calendar", "Calendar", "Schedule placeholder", "\uE787"),
            new("Reports", "Reports", "Placeholder", "\uE9F9"),
            new("Settings", "Settings", "Application setup", "\uE713")
        };

        ToggleSidebarCommand = new RelayCommand(_ => ToggleSidebar());
        SelectNavigationCommand = new RelayCommand(SelectNavigation);

        SetActiveNavigation("Dashboard");
        LogoImagePath = ResolveLogoPath();
        ApplicationName = configurationService.ApplicationName;
        Version = configurationService.Version;
        EnvironmentName = configurationService.EnvironmentName;
        ThemeName = themeService.CurrentThemeName;

        _navigationService.CurrentPageChanged += (_, page) => CurrentPage = page;
    }

    public string CompanyName { get; } = "Smarto Life";

    public string Slogan { get; } = "Smart Solutions for Tomorrow";

    public string PhaseLabel { get; } = "v0.3.0 Professional CRM Shell";

    public string ApplicationName { get; }

    public string Version { get; }

    public string EnvironmentName { get; }

    public string ThemeName { get; }

    public string UserDisplayName { get; } = "Admin User";

    public string UserRoleLabel { get; } = "Local CRM";

    public string StatusText => $"{Version} | {EnvironmentName} | SQLite ready | {ThemeName}";

    public string? LogoImagePath { get; }

    public Visibility LogoVisibility => string.IsNullOrWhiteSpace(LogoImagePath) ? Visibility.Collapsed : Visibility.Visible;

    public Visibility PlaceholderLogoVisibility => string.IsNullOrWhiteSpace(LogoImagePath) ? Visibility.Visible : Visibility.Collapsed;

    public bool IsSidebarExpanded
    {
        get => _isSidebarExpanded;
        set
        {
            if (SetProperty(ref _isSidebarExpanded, value))
            {
                OnPropertyChanged(nameof(SidebarTextVisibility));
                OnPropertyChanged(nameof(SidebarFooterVisibility));
                OnPropertyChanged(nameof(SidebarToggleGlyph));
            }
        }
    }

    public Visibility SidebarTextVisibility => IsSidebarExpanded ? Visibility.Visible : Visibility.Collapsed;

    public Visibility SidebarFooterVisibility => IsSidebarExpanded ? Visibility.Visible : Visibility.Collapsed;

    public string SidebarToggleGlyph => IsSidebarExpanded ? "\uE76B" : "\uE76C";

    public string GlobalSearchText
    {
        get => _globalSearchText;
        set => SetProperty(ref _globalSearchText, value);
    }

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    public PageViewModelBase CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (SetProperty(ref _currentPage, value))
            {
                CurrentModulePage = value as ModulePageViewModel;
                OnPropertyChanged(nameof(DashboardPage));
                OnPropertyChanged(nameof(CurrentSections));
                OnPropertyChanged(nameof(CurrentRecords));
                OnPropertyChanged(nameof(IsDashboardVisible));
                OnPropertyChanged(nameof(IsModulePageVisible));
                SetActiveNavigation(value.ModuleKey);
            }
        }
    }

    public DashboardViewModel? DashboardPage => CurrentPage as DashboardViewModel;

    public ModulePageViewModel? CurrentModulePage
    {
        get => _currentModulePage;
        private set => SetProperty(ref _currentModulePage, value);
    }

    public IEnumerable<ModuleSectionViewModel> CurrentSections =>
        CurrentPage switch
        {
            ModulePageViewModel modulePage => modulePage.Sections,
            SettingsPageViewModel settingsPage => settingsPage.Sections,
            _ => Enumerable.Empty<ModuleSectionViewModel>()
        };

    public IEnumerable<RecentRecordViewModel> CurrentRecords =>
        CurrentModulePage?.Records ?? Enumerable.Empty<RecentRecordViewModel>();

    public Visibility IsDashboardVisible => CurrentPage is DashboardViewModel ? Visibility.Visible : Visibility.Collapsed;

    public Visibility IsModulePageVisible => CurrentPage is DashboardViewModel ? Visibility.Collapsed : Visibility.Visible;

    public ICommand ToggleSidebarCommand { get; }

    public ICommand SelectNavigationCommand { get; }

    private void ToggleSidebar()
    {
        IsSidebarExpanded = !IsSidebarExpanded;
    }

    private void SelectNavigation(object? parameter)
    {
        if (parameter is NavigationItemViewModel navigationItem)
        {
            _navigationService.NavigateTo(navigationItem.ModuleKey);
        }
        else if (parameter is string moduleKey)
        {
            _navigationService.NavigateTo(moduleKey);
        }
    }

    private void SetActiveNavigation(string title)
    {
        foreach (var item in NavigationItems)
        {
            item.IsActive = item.ModuleKey == title;
        }
    }

    private static string? ResolveLogoPath()
    {
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Logo", "SmartoLife_Logo.png");
        return File.Exists(logoPath) ? logoPath : null;
    }
}


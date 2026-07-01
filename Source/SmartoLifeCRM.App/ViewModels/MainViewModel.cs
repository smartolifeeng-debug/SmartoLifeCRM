using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SmartoLifeCRM.App.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private string _selectedModule = "Dashboard";
    private bool _isSidebarExpanded = true;

    public MainViewModel()
    {
        NavigationItems = new ObservableCollection<NavigationItemViewModel>
        {
            new("Dashboard", "Executive overview", "\uE80F", true),
            new("Contacts", "Customers, suppliers, leads", "\uE77B", true),
            new("Products", "Catalog foundation", "\uE7BF", true),
            new("Quotations", "Quotation foundation", "\uE8A5", true),
            new("Categories", "Business types", "\uE8EC", true),
            new("Tags", "Interests and topics", "\uE8EC", true),
            new("Business Matters", "Interaction topics", "\uE8F1", true),
            new("Settings", "Company profile", "\uE713", true),
            new("Marketing", "Future phase", "\uE789", false),
            new("Map", "Future phase", "\uE707", false),
            new("Scans", "Future phase", "\uE8B6", false)
        };

        Metrics = new ObservableCollection<DashboardMetricViewModel>
        {
            new("Total Contacts", "0", "Unified contact foundation", "\uE77B"),
            new("Customers", "0", "Role filter ready", "\uE716"),
            new("Suppliers", "0", "Shared contact model", "\uE8FD"),
            new("Leads", "0", "Lead role ready", "\uE8A7"),
            new("Products", "0", "Catalog foundation", "\uE7BF"),
            new("Quotations", "0", "Line-item ready", "\uE8A5")
        };

        FoundationModules = new ObservableCollection<ModuleSummaryViewModel>
        {
            new("Company Profile", "Single source of truth for Smarto Life details and branding.", "Foundation ready"),
            new("Unified Contacts", "One model supports customers, suppliers, leads, and combined roles.", "Foundation ready"),
            new("Category System", "Dynamic business type lookup.", "Foundation ready"),
            new("Tag System", "Dynamic interest and product-topic lookup.", "Foundation ready"),
            new("Business Matter System", "Dynamic interaction and pipeline-topic lookup.", "Foundation ready"),
            new("Product Module", "Catalog, category, tags, pricing, stock, images, and datasheet fields.", "Phase 2.1 ready"),
            new("Quotation Module", "Quotation headers and product-ready line items linked to unified contacts.", "Phase 2.1 ready"),
            new("SQLite Database", "Local-first EF Core database foundation.", "Configured")
        };

        QuickActions = new ObservableCollection<QuickActionViewModel>
        {
            new("Add Contact", "\uE710", "Open the unified contact form when forms are enabled."),
            new("New Product", "\uE7BF", "Prepare product catalog entry workflow."),
            new("New Quotation", "\uE8A5", "Prepare quotation workflow linked to contacts."),
            new("Manage Lookups", "\uE8EC", "Categories, tags, and business matters.")
        };

        RecentActivities = new ObservableCollection<ActivityItemViewModel>
        {
            new("Phase 1 locked", "Application foundation verified and approved.", "Today"),
            new("Phase 2.1 accepted", "Products and quotations foundation accepted as backend milestone.", "Today"),
            new("UI modernization started", "Professional shell and dashboard redesign in progress.", "Now")
        };

        UpcomingTasks = new ObservableCollection<TaskItemViewModel>
        {
            new("Review modernized dashboard", "Phase 2A", "Pending review"),
            new("Validate 1080p layout", "Manual QA", "Ready"),
            new("Validate high-DPI scaling", "Manual QA", "Ready")
        };

        ChartPlaceholders = new ObservableCollection<ChartPlaceholderViewModel>
        {
            new("Contact Growth", "Future chart area reserved for CRM analytics.", "No data yet"),
            new("Quotation Pipeline", "Future chart area for draft, sent, and accepted quotations.", "No data yet")
        };

        ToggleSidebarCommand = new RelayCommand(_ => ToggleSidebar());
        SelectNavigationCommand = new RelayCommand(SelectNavigation, item => item is NavigationItemViewModel { IsEnabled: true });

        SetActiveNavigation("Dashboard");
        LogoImagePath = ResolveLogoPath();
    }

    public string CompanyName { get; } = "Smarto Life";

    public string Slogan { get; } = "Smart Solutions for Tomorrow";

    public string PhaseLabel { get; } = "Phase 2A UI Modernization";

    public string UserDisplayName { get; } = "Admin User";

    public string UserRoleLabel { get; } = "Local CRM";

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

    public string SelectedModule
    {
        get => _selectedModule;
        set
        {
            if (SetProperty(ref _selectedModule, value))
            {
                SetActiveNavigation(value);
            }
        }
    }

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    public ObservableCollection<DashboardMetricViewModel> Metrics { get; }

    public ObservableCollection<ModuleSummaryViewModel> FoundationModules { get; }

    public ObservableCollection<QuickActionViewModel> QuickActions { get; }

    public ObservableCollection<ActivityItemViewModel> RecentActivities { get; }

    public ObservableCollection<TaskItemViewModel> UpcomingTasks { get; }

    public ObservableCollection<ChartPlaceholderViewModel> ChartPlaceholders { get; }

    public ICommand ToggleSidebarCommand { get; }

    public ICommand SelectNavigationCommand { get; }

    private void ToggleSidebar()
    {
        IsSidebarExpanded = !IsSidebarExpanded;
    }

    private void SelectNavigation(object? parameter)
    {
        if (parameter is NavigationItemViewModel navigationItem && navigationItem.IsEnabled)
        {
            SelectedModule = navigationItem.Title;
        }
    }

    private void SetActiveNavigation(string title)
    {
        foreach (var item in NavigationItems)
        {
            item.IsActive = item.Title == title;
        }
    }

    private static string? ResolveLogoPath()
    {
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Logo", "SmartoLife_Logo.png");
        return File.Exists(logoPath) ? logoPath : null;
    }
}


using SmartoLifeCRM.App.ViewModels;

namespace SmartoLifeCRM.App.Services;

public sealed class NavigationService : INavigationService
{
    private readonly Dictionary<string, PageViewModelBase> _pages;

    public NavigationService(DashboardViewModel dashboardViewModel, SettingsPageViewModel settingsPageViewModel)
    {
        _pages = new Dictionary<string, PageViewModelBase>(StringComparer.OrdinalIgnoreCase)
        {
            ["Dashboard"] = dashboardViewModel,
            ["Contacts"] = CreateModulePage(
                "Contacts",
                "Contacts",
                "Unified customers, suppliers, leads, and other contact records.",
                "\uE77B",
                "Unified list shell",
                "Contact details placeholder",
                "Customer and supplier role filters"),
            ["Products"] = CreateModulePage(
                "Products",
                "Products",
                "Product catalog shell using the accepted backend foundation.",
                "\uE7BF",
                "Catalog list placeholder",
                "Product categories and tags",
                "Quotation line item readiness"),
            ["Quotations"] = CreateModulePage(
                "Quotations",
                "Quotations",
                "Quotation workspace placeholder linked to unified contacts.",
                "\uE8A5",
                "Quotation list placeholder",
                "Draft status shell",
                "Line item readiness"),
            ["Calendar"] = CreateModulePage(
                "Calendar",
                "Calendar",
                "Calendar shell placeholder for future scheduling review.",
                "\uE787",
                "Month view placeholder",
                "Upcoming tasks sample",
                "No calendar module implemented"),
            ["Reports"] = CreateModulePage(
                "Reports",
                "Reports",
                "Reports placeholder only. No reporting engine implemented.",
                "\uE9F9",
                "Report gallery placeholder",
                "Export placeholder",
                "No report generation implemented"),
            ["Settings"] = settingsPageViewModel
        };

        CurrentPage = dashboardViewModel;
    }

    public PageViewModelBase CurrentPage { get; private set; }

    public event EventHandler<PageViewModelBase>? CurrentPageChanged;

    public void NavigateTo(string moduleKey)
    {
        if (!_pages.TryGetValue(moduleKey, out var page))
        {
            return;
        }

        CurrentPage = page;
        CurrentPageChanged?.Invoke(this, CurrentPage);
    }

    private static ModulePageViewModel CreateModulePage(
        string moduleKey,
        string title,
        string subtitle,
        string iconGlyph,
        string firstSection,
        string secondSection,
        string thirdSection)
    {
        return new ModulePageViewModel(
            moduleKey,
            title,
            subtitle,
            iconGlyph,
            new[]
            {
                new ModuleSectionViewModel(firstSection, "Professional placeholder area prepared for later approved development.", "Ready"),
                new ModuleSectionViewModel(secondSection, "UI shell only. No business workflow has been added.", "Placeholder"),
                new ModuleSectionViewModel(thirdSection, "Architecture is ready without expanding functional scope.", "Planned")
            },
            new[]
            {
                new RecentRecordViewModel($"{title} sample 01", "Placeholder sample row", "Sample"),
                new RecentRecordViewModel($"{title} sample 02", "Placeholder sample row", "Sample"),
                new RecentRecordViewModel($"{title} sample 03", "Placeholder sample row", "Sample")
            });
    }
}


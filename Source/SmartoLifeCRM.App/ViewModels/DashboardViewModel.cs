using System.Collections.ObjectModel;

namespace SmartoLifeCRM.App.ViewModels;

public sealed class DashboardViewModel : PageViewModelBase
{
    public DashboardViewModel()
        : base("Dashboard", "Dashboard", "Executive overview with sample CRM shell data.", "\uE80F")
    {
        Metrics = new ObservableCollection<DashboardMetricViewModel>
        {
            new("Total Contacts", "128", "Sample unified records", "\uE77B"),
            new("Customers", "74", "Role-filtered contacts", "\uE716"),
            new("Suppliers", "19", "Shared contact model", "\uE8FD"),
            new("Products", "42", "Catalog placeholders", "\uE7BF"),
            new("Quotations", "11", "Draft and active samples", "\uE8A5"),
            new("Tasks Today", "6", "Sample reminders", "\uE121")
        };

        QuickActions = new ObservableCollection<QuickActionViewModel>
        {
            new("Add Contact", "\uE710", "Prepare a unified contact workflow."),
            new("New Product", "\uE7BF", "Prepare product catalog workflow."),
            new("New Quotation", "\uE8A5", "Prepare quotation workflow."),
            new("Open Settings", "\uE713", "Review shell and company settings.")
        };

        RecentActivities = new ObservableCollection<ActivityItemViewModel>
        {
            new("CRM shell upgraded", "Version 0.3.0 professional shell prepared.", "Now"),
            new("Phase 2A review", "UI modernization is ready for local validation.", "Today"),
            new("Phase 2.1 locked", "Products and quotations remain backend foundations only.", "Today")
        };

        UpcomingTasks = new ObservableCollection<TaskItemViewModel>
        {
            new("Review desktop layout", "Today", "Ready"),
            new("Check sidebar collapse", "Today", "Ready"),
            new("Validate high DPI", "This week", "Planned")
        };

        RecentCustomers = new ObservableCollection<RecentRecordViewModel>
        {
            new("Al Noor Trading", "Dubai - Customer", "Active"),
            new("Smart Tech Market", "Sharjah - Lead", "Follow-up"),
            new("City Electronics", "Ajman - Customer", "New")
        };

        RecentQuotations = new ObservableCollection<RecentRecordViewModel>
        {
            new("Q-2026-001", "Al Noor Trading", "Draft"),
            new("Q-2026-002", "City Electronics", "Prepared"),
            new("Q-2026-003", "Smart Tech Market", "Review")
        };

        ChartPlaceholders = new ObservableCollection<ChartPlaceholderViewModel>
        {
            new("Customer Growth", "Future analytics placeholder.", "Sample trend"),
            new("Quotation Pipeline", "Future quotation status chart placeholder.", "Sample pipeline")
        };
    }

    public string TodaySummary { get; } = "Sample dashboard only: 6 tasks, 3 recent customers, 3 quotation placeholders.";

    public ObservableCollection<DashboardMetricViewModel> Metrics { get; }

    public ObservableCollection<QuickActionViewModel> QuickActions { get; }

    public ObservableCollection<ActivityItemViewModel> RecentActivities { get; }

    public ObservableCollection<TaskItemViewModel> UpcomingTasks { get; }

    public ObservableCollection<RecentRecordViewModel> RecentCustomers { get; }

    public ObservableCollection<RecentRecordViewModel> RecentQuotations { get; }

    public ObservableCollection<ChartPlaceholderViewModel> ChartPlaceholders { get; }
}


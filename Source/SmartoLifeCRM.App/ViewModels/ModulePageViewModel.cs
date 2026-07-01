using System.Collections.ObjectModel;

namespace SmartoLifeCRM.App.ViewModels;

public sealed class ModulePageViewModel : PageViewModelBase
{
    public ModulePageViewModel(
        string moduleKey,
        string title,
        string subtitle,
        string iconGlyph,
        IEnumerable<ModuleSectionViewModel> sections,
        IEnumerable<RecentRecordViewModel> records)
        : base(moduleKey, title, subtitle, iconGlyph)
    {
        Sections = new ObservableCollection<ModuleSectionViewModel>(sections);
        Records = new ObservableCollection<RecentRecordViewModel>(records);
    }

    public ObservableCollection<ModuleSectionViewModel> Sections { get; }

    public ObservableCollection<RecentRecordViewModel> Records { get; }
}


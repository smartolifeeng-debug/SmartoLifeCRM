using System.Windows;
using SmartoLifeCRM.App.ViewModels;

namespace SmartoLifeCRM.App;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}


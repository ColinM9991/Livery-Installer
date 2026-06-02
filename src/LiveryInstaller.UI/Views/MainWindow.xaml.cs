using System.Windows;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
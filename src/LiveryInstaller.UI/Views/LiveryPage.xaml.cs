using System.Windows;
using System.Windows.Controls;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Views;

public partial class LiveryPage : UserControl
{
    public LiveryPage()
    {
        InitializeComponent();
    }

    private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: LiveryViewModel viewModel })
            return;

        if (viewModel.LoadIconCommand.CanExecute(null))
            await viewModel.LoadIconCommand.ExecuteAsync(null);
    }
}
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models.Toast;

namespace LiveryInstaller.UI.UserControls;

public partial class ToastCard : UserControl
{
    public ToastCard()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
        nameof(Level), typeof(ToastLevel), typeof(ToastCard), new PropertyMetadata(default(ToastLevel)));

    public ToastLevel Level
    {
        get => (ToastLevel)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(ToastCard), new PropertyMetadata(default(string)));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(ToastCard), new PropertyMetadata(default(string)));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly DependencyProperty TitleVisibilityProperty = DependencyProperty.Register(
        nameof(TitleVisibility), typeof(bool), typeof(ToastCard), new PropertyMetadata(false));

    public bool TitleVisibility
    {
        get => (bool)GetValue(TitleVisibilityProperty);
        set => SetValue(TitleVisibilityProperty, value);
    }

    public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
        nameof(CloseCommand), typeof(IRelayCommand), typeof(ToastCard), new PropertyMetadata(default(IRelayCommand)));

    public IRelayCommand CloseCommand
    {
        get => (IRelayCommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }
}
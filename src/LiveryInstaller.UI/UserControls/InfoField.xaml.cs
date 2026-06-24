using System.Windows;
using System.Windows.Controls;

namespace LiveryInstaller.UI.UserControls;

public partial class InfoField : UserControl
{
    public InfoField()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(InfoField), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(InfoField), new PropertyMetadata(default(string)));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
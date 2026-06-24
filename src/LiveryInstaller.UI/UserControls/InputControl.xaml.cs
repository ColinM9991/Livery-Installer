using System.Windows;
using System.Windows.Controls;

namespace LiveryInstaller.UI.UserControls;

public partial class InputControl : UserControl
{
    public InputControl() => InitializeComponent();

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(string), typeof(InputControl), new PropertyMetadata(null));

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(InputControl), new PropertyMetadata(null));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(InputControl), new PropertyMetadata(null));

    public static readonly DependencyProperty CardContentProperty = DependencyProperty.Register(
        nameof(CardContent), typeof(object), typeof(InputControl), new PropertyMetadata(null));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public object CardContent
    {
        get => GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }
}
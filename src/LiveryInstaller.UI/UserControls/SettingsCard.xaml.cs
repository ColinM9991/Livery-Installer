using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace LiveryInstaller.UI.UserControls;

[ContentProperty(nameof(CardContent))]
public partial class SettingsCard : UserControl
{
    public SettingsCard()
    {
        InitializeComponent();
    }

    private static DependencyProperty IconProperty { get; } = DependencyProperty.Register(
        "Icon", typeof(string), typeof(SettingsCard), new PropertyMetadata(null));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    private static DependencyProperty TitleProperty { get; } = DependencyProperty.Register(
        "Title", typeof(string), typeof(SettingsCard), new PropertyMetadata(null));
    
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    private static DependencyProperty DescriptionProperty { get; } = DependencyProperty.Register(
        "Description", typeof(string), typeof(SettingsCard), new PropertyMetadata(null));
    
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    
    private static DependencyProperty CardContentProperty { get; } = DependencyProperty.Register(
        "CardContent", typeof(object), typeof(SettingsCard), new PropertyMetadata(null));
    
    public object CardContent
    {
        get => GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }
}
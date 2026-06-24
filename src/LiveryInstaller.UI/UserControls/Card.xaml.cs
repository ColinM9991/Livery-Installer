using System.Windows;
using System.Windows.Controls;

namespace LiveryInstaller.UI.UserControls;

public partial class Card : UserControl
{
    public Card()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(Card), new PropertyMetadata(null, OnTitleChanged));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(Card), new PropertyMetadata(null, OnDescriptionChanged));

    public static readonly DependencyProperty CardContentProperty = DependencyProperty.Register(
        nameof(CardContent), typeof(object), typeof(Card), new PropertyMetadata(null));

    public static readonly DependencyProperty CardFooterProperty = DependencyProperty.Register(
        nameof(CardFooter), typeof(object), typeof(Card), new PropertyMetadata(null, OnFooterChanged));

    private static readonly DependencyProperty TitleVisibilityProperty =
        DependencyProperty.Register(
            nameof(TitleVisibility), typeof(Visibility), typeof(Card),
            new PropertyMetadata(Visibility.Collapsed));

    private static readonly DependencyProperty DescriptionVisibilityProperty = DependencyProperty.Register(
        nameof(DescriptionVisibility), typeof(Visibility), typeof(Card),
        new PropertyMetadata(Visibility.Collapsed));

    private static readonly DependencyProperty CardFooterVisibilityProperty = DependencyProperty.Register(
        nameof(CardFooterVisibility), typeof(Visibility), typeof(Card), new PropertyMetadata(Visibility.Collapsed));

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
        get => (object)GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }

    public object CardFooter
    {
        get => (object)GetValue(CardFooterProperty);
        set => SetValue(CardFooterProperty, value);
    }

    private Visibility TitleVisibility
    {
        get => (Visibility)GetValue(TitleVisibilityProperty);
        set => SetValue(TitleVisibilityProperty, value);
    }

    private Visibility DescriptionVisibility
    {
        get => (Visibility)GetValue(DescriptionVisibilityProperty);
        set => SetValue(DescriptionVisibilityProperty, value);
    }

    private Visibility CardFooterVisibility
    {
        get => (Visibility)GetValue(CardFooterVisibilityProperty);
        set => SetValue(CardFooterVisibilityProperty, value);
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (Card)d;
        card.TitleVisibility = string.IsNullOrWhiteSpace((string)e.NewValue)
            ? Visibility.Hidden
            : Visibility.Visible;
    }

    private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (Card)d;
        card.DescriptionVisibility = string.IsNullOrWhiteSpace((string)e.NewValue)
            ? Visibility.Hidden
            : Visibility.Visible;
    }

    private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (Card)d;
        card.CardFooterVisibility = e.NewValue is null
            ? Visibility.Hidden
            : Visibility.Visible;
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace LiveryInstaller.UI.UserControls;

public partial class FilePickerControl : UserControl
{
    public enum PickerType
    {
        Folder,
        File
    }

    public FilePickerControl()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register(
            nameof(Filter), typeof(string), typeof(FilePickerControl),
            new PropertyMetadata("All Files|*.*"));

    public static readonly DependencyProperty PathProperty =
        DependencyProperty.Register(
            nameof(Path), typeof(string), typeof(FilePickerControl),
            new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type), typeof(PickerType), typeof(FilePickerControl), new PropertyMetadata(default(PickerType)));

    public string Filter
    {
        get => (string)GetValue(FilterProperty);
        set => SetValue(FilterProperty, value);
    }

    public string Path
    {
        get => (string)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    public PickerType Type
    {
        get => (PickerType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        CommonItemDialog dialog = Type switch
        {
            PickerType.File => new OpenFileDialog
            {
                Filter = Filter,
                Multiselect = false
            },
            PickerType.Folder => new OpenFolderDialog { Multiselect = false },
            _ => throw new ArgumentOutOfRangeException(nameof(Type))
        };

        var result = dialog.ShowDialog();
        if (!result.GetValueOrDefault()) return;

        Path = dialog switch
        {
            OpenFileDialog fileDialog => fileDialog.FileName,
            OpenFolderDialog folderDialog => folderDialog.FolderName,
            _ => throw new ArgumentOutOfRangeException(nameof(dialog))
        };
    }
}
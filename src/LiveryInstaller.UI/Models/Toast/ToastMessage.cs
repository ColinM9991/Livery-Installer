namespace LiveryInstaller.UI.Models.Toast;

public record ToastMessage(string Description, ToastLevel Level)
{
    public ToastMessage(
        string title,
        string description,
        ToastLevel level) : this(description, level)
    {
        Title = title;
    }

    public string Title { get; set; }
}
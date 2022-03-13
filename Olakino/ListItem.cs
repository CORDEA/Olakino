namespace Olakino;

public class ListItem
{
    public ListItem(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; }
    public string Description { get; }
}

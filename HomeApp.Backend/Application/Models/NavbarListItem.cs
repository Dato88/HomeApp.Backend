namespace Application.Models;

public sealed class NavbarListItem
{
    public string Name { get; init; } = null;
    public string Icon { get; init; } = null;
    public string Link { get; init; } = null;

    public List<NavbarListSubitem> Sublist { get; set; }
}

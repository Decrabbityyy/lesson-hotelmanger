using HotelManager.Cli.Utilities.Menus;

namespace HotelManager.Cli.MenuItems;

internal class BackMenuItem(IMenu menu, object? context) : IMenuItem
{
    private readonly object? _context = context;

    string IMenuItem.GetTitle(ApplicationContext application, object? context)
    {
        return "返回";
    }

    (IMenu?, object?) IMenuItem.Run(ApplicationContext application, object? context)
    {
        return (menu, _context);
    }
}
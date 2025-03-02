using HotelManager.Cli.Utilities.Menus;

namespace HotelManager.Cli.MenuItems;

internal class ExitMenuItem : IMenuItem
{
    public static readonly ExitMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, object? context)
    {
        return "退出";
    }

    public (IMenu?, object?) Run(ApplicationContext application, object? context)
    {
        return (null, null);
    }
}
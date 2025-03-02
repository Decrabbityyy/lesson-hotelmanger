using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;

namespace HotelManager.Cli.Menus;

internal class MainMenu : IMenu<object>
{
    public static readonly MainMenu Singleton  = new();

    public string GetTitle(ApplicationContext application, object? context)
    {
        return "首页";
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, object? context)
    {
        return
        [
            LoginMenuItem.Singleton,
            RegisterMenuItem.Singleton,
            ExitMenuItem.Singleton
        ];
    }
}
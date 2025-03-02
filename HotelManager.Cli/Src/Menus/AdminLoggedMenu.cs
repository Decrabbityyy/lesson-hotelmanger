using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.Menus;

internal class AdminLoggedMenu : IMenu<User>
{
    public static readonly AdminLoggedMenu Singleton = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user != null
            ? $"主菜单 | 管理员 {user.Name}"
            : throw new Exception("user is null");
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, User? context)
    {
        return
        [
            UserManagerMenuItem.Singleton,
            RoomManagerMenuItem.Singleton,
            new BackMenuItem(MainMenu.Singleton, null)
        ];
    }
}
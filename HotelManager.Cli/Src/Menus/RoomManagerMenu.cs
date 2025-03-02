using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.Menus;

internal class RoomManagerMenu : IMenu<User>
{
    public static readonly RoomManagerMenu Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user != null
            ? $"房间管理 | 管理员 {user.Name}"
            : throw new Exception("user is null");
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, User? user)
    {
        return
        [
            AddSingleRoomMenuItem.Singleton,
            AddDoubleRoomMenuItem.Singleton,
            AddSuiteRoomMenuItem.Singleton,
            RemoveRoomMenuItem.Singleton,
            OverviewRoomMenuItem.Singleton, 
            new BackMenuItem(AdminLoggedMenu.Singleton, user)
        ];
    }
}
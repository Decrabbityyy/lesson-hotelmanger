using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class RoomManagerMenuItem : IMenuItem<User>
{
    public static readonly RoomManagerMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "房间管理";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? context)
    {
        return (RoomManagerMenu.Singleton, context);
    }
}
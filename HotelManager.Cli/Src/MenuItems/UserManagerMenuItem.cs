using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class UserManagerMenuItem : IMenuItem<User>
{
    public static readonly UserManagerMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "用户管理";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? context)
    {
        return (UserManagerMenu.Singleton, context);
    }
}
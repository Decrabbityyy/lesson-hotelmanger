using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class CheckInMenuItem : IMenuItem<User>
{
    public static readonly CheckInMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "入住";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        return (CheckInMenu.Singleton, user);
    }
}
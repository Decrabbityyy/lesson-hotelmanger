using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.Menus;

internal class UserLoggedMenu : IMenu<User>
{
    public static UserLoggedMenu Singleton { get; } = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user != null
            ? $"主菜单 | {RoleDisplay(user.Role)} {user.Name} | 余额 {user.Money}"
            : throw new Exception("user is null");
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, User? context)
    {
        return
        [
            CheckInMenuItem.Singleton,
            CheckOutMenuItem.Singleton,
            RechargeMenuItem.Singleton,
            new BackMenuItem(MainMenu.Singleton, null)
        ];
    }

    private static string RoleDisplay(UserRole role)
    {
        return role switch
        {
            UserRole.Normal => "用户",
            UserRole.VIP1 => "Lv.1 会员",
            UserRole.VIP2 => "Lv.2 会员",
            UserRole.VIP3 => "Lv.3 会员",
            UserRole.VIP4 => "Lv.4 会员",
            UserRole.VIP5 => "Lv.5 会员",
            _ => throw new NotImplementedException()
        };
    }
}
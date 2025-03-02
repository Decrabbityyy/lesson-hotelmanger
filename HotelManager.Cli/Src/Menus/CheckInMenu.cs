using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.Menus;

internal class CheckInMenu : IMenu<User>
{
    public static CheckInMenu Singleton { get; } = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user != null
            ? $"入住 | {RoleDisplay(user.Role)} {user.Name}"
            : throw new Exception("user is null");
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, User? user)
    {
        return
        [
            CheckInSingleMenuItem.Singleton,
            CheckInDoubleMenuItem.Singleton,
            CheckInSuiteMenuItem.Singleton,
            new BackMenuItem(UserLoggedMenu.Singleton, user)
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
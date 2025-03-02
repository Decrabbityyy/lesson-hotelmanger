using HotelManager.Cli.MenuItems;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.Menus;

internal class UserManagerMenu : IMenu<User>
{
    public static readonly UserManagerMenu Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user != null
            ? $"用户管理 | 管理员 {user.Name}"
            : throw new Exception("user is null");
    }

    public IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, User? user)
    {
        return
        [
            AddUserMenuItem.Singleton,
            AddAdminMenuItem.Singleton,
            UpgradeUserMenuItem.Singleton,
            DowngradeUserMenuItem.Singleton,
            RemoveUserMenuItem.Singleton,
            ResetUserPasswordMenuItem.Singleton,
            new BackMenuItem(AdminLoggedMenu.Singleton, user)
        ];
    }
}
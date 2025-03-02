using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class LoginMenuItem : IMenuItem
{
    public static readonly LoginMenuItem Singleton = new();

    public string GetTitle(ApplicationContext application, object? context)
    {
        return "登录";
    }

    public (IMenu?, object?) Run(ApplicationContext application, object? context)
    {
        User? user;
        while (true)
        {
            // 读取用户名
            string? username;
            while (true)
            {
                Console.Write("| 请输入用户名 [.q 退出]: ");
                username = Console.ReadLine();

                if (username == ".q") return (MainMenu.Singleton, null);
                if (username == null)
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }

                break;
            }

            // 读取密码
            string? password;
            while (true)
            {
                Console.Write("| 请输入密码 [.q 退出]: ");
                password = Console.ReadLine();

                if (password == ".q") return (MainMenu.Singleton, null);
                if (password == null)
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }

                break;
            }

            // 获取用户
            user = application.UserService.GetUser(username);
            if (user == null || user.Password != password)
            {
                Console.WriteLine("| [ WARN] 用户名或密码错误!");
                continue;
            }

            break;
        }

        return user.Role switch
        {
            UserRole.Admin => (AdminLoggedMenu.Singleton, user),
            UserRole.Normal or
                UserRole.VIP1 or
                UserRole.VIP2 or
                UserRole.VIP3 or
                UserRole.VIP4 or
                UserRole.VIP5 => (UserLoggedMenu.Singleton, user),
            _ => throw new NotImplementedException()
        };
    }
}
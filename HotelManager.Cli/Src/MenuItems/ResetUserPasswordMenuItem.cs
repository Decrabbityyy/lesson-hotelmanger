using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class ResetUserPasswordMenuItem : IMenuItem<User>
{
    public static readonly ResetUserPasswordMenuItem Singleton = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return "修改用户密码";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? context)
    {
        if (context == null) throw new Exception("user is null");
        string? username;
        string? password;

        // 读取用户名
        while (true)
        {
            Console.Write("| 请输入用户名 [.q 退出]: ");
            username = Console.ReadLine();

            if (username == ".q") return (UserManagerMenu.Singleton, null);
            if (username == null)
            {
                Console.WriteLine("| [ WARN] 输入不合法");
                continue;
            }

            if (!application.UserService.UserExists(username))
            {
                Console.WriteLine("| [ WARN] 用户不存在");
                continue;
            }
            

            break;
        }

        // 读取密码
        while (true)
        {
            Console.Write("| 请输入密码 [.q 退出]: ");
            password = Console.ReadLine();

            if (password == ".q") return (UserManagerMenu.Singleton, null);
            if (password == null)
            {
                Console.WriteLine("| [ WARN] 输入不合法");
                continue;
            }

            break;
        }

        application.UserService.ChangePassword(username, password);
        return (UserManagerMenu.Singleton, context);
    }
}
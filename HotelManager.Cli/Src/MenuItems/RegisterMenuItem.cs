using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class RegisterMenuItem : IMenuItem
{
    public static readonly RegisterMenuItem Singleton = new();

    public string GetTitle(ApplicationContext application, object? context)
    {
        return "注册";
    }

    public (IMenu?, object?) Run(ApplicationContext application, object? context)
    {
        while (true)
        {
            // 读取用户名
            string? username;
            while (true)
            {
                Console.Write("| 请输入用户名 [.q 退出]: ");
                username = Console.ReadLine();

                if (username == ".q") return (MainMenu.Singleton, context);
                if (username == null)
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }

                // 判断是否存在
                if (application.UserService.GetUser(username) != null)
                {
                    Console.WriteLine("| [ WARN] 用户名已存在");
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

                if (password == ".q") return (MainMenu.Singleton, context);
                if (password == null)
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }


                Console.Write("| 请再次输入密码 [.q 退出]: ");
                var check = Console.ReadLine();

                if (check == ".q") return (MainMenu.Singleton, context);
                if (password != check)
                {
                    Console.WriteLine("| [ WARN] 两次密码不匹配!");
                    continue;
                }

                break;
            }

            try
            {
                application.UserService.CreateUser(username, password, UserRole.Normal);
            }
            catch (UserExistsException)
            {
                Console.WriteLine("| [ WARN] 用户名已存在");
                continue;
            }

            Console.WriteLine("| [ INFO] 注册成功");
            return (MainMenu.Singleton, context);
        }
    }
}
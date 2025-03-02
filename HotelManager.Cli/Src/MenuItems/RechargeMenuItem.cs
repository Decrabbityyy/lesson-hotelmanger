using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class RechargeMenuItem : IMenuItem<User>
{
    public static RechargeMenuItem Singleton { get; } = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return "充值";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        if (user == null) throw new Exception("user is null");

        long money;
        while (true)
        {
            Console.Write("| 请输入充入数值 [.q 退出]: ");
            var input = Console.ReadLine();

            if (input == ".q") return (UserLoggedMenu.Singleton, user);
            if (input == null || !long.TryParse(input, out money))
            {
                Console.WriteLine("| [ WARN] 输入不合法");
                continue;
            }

            break;
        }

        application.UserService.RechargeUser(user.Name, money);
        Console.WriteLine("充值成功");
        user.Money += money;
        return (UserLoggedMenu.Singleton, user);
    }
}
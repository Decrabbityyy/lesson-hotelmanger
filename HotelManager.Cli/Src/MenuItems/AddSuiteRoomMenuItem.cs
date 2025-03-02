using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class AddSuiteRoomMenuItem : IMenuItem<User>
{
    public static readonly AddSuiteRoomMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return "添加套房";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        while (true)
        {
            long roomId;
            while (true)
            {
                Console.Write("| 请输入房间号 [.q 退出]: ");
                var input = Console.ReadLine();

                if (input == ".q") return (RoomManagerMenu.Singleton, user);
                if (input == null || !long.TryParse(input, out roomId))
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }

                // 判断是否存在
                if (application.RoomService.GetRoom(roomId) != null)
                {
                    Console.WriteLine("| [ WARN] 房间号已存在");
                    continue;
                }

                break;
            }

            long price;
            while (true)
            {
                Console.Write("请输入价格 [.q 退出]: ");
                var input = Console.ReadLine();

                if (input == ".q") return (RoomManagerMenu.Singleton, user);
                if (input == null || !long.TryParse(input, out price))
                {
                    Console.WriteLine("| [ WARN] 输入不合法");
                    continue;
                }

                break;
            }

            try
            {
                application.RoomService.CreateRoom(roomId, RoomType.Suite, price);
            }
            catch (RoomExistsException)
            {
                Console.WriteLine("| [ WARN] 房间号已存在");
                continue;
            }

            return (RoomManagerMenu.Singleton, user);
        }
    }
}
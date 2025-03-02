using System.Text;
using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class CheckOutMenuItem : IMenuItem<User>
{
    public static readonly CheckOutMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "退房";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        if (user == null) throw new Exception("user is null");
        
        Console.CursorVisible = false;
        try
        {
            Order[] orders = application.OrderService.GetCheckInOrders(0, 5);
            if (orders.Length == 0)
            {
                Console.WriteLine("| [ INFO] 你没有订房间");
                return (UserLoggedMenu.Singleton, user);
            }

            var selected = 0;

            while (true)
            {
                Console.WriteLine("+ ------------------");
                for (var i = 0; i < orders.Length; i++)
                    Console.WriteLine(new StringBuilder()
                        .Append("| ")
                        .Append(i == selected ? '>' : ' ')
                        .Append(' ')
                        .Append(orders[i].Room.Id)
                        .Append(" | ")
                        .Append(orders[i].Room.Type));
                Console.WriteLine("+ ------------------");
                Console.Write("| ↑↓ 移动光标 ←→ 翻页 Enter 确认 q 退出");

                Console.CursorTop -= orders.Length + 2;
                Console.CursorLeft = 0;

                var key = Console.ReadKey(true);
                if (key.Modifiers != ConsoleModifiers.None) continue;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        selected = Math.Max(selected - 1, 0);
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        selected = Math.Min(selected + 1, orders.Length - 1);
                        break;
                    }
                    case ConsoleKey.LeftArrow:
                    {
                        Order[] prev = application.OrderService.GetCheckInOrders(orders[0].RoomId, -5);
                        if (prev.Length == 0) break;

                        Console.CursorTop += orders.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        orders = prev;
                        selected = Math.Max(0, Math.Min(selected, orders.Length));
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        Order[] next = application.OrderService.GetCheckInOrders(orders[^1].RoomId, 5);
                        if (next.Length == 0) break;

                        Console.CursorTop += orders.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        orders = next;
                        selected = Math.Max(0, Math.Min(selected, orders.Length));
                        break;
                    }
                    case ConsoleKey.Q:
                    {
                        Console.CursorTop += orders.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        return (UserLoggedMenu.Singleton, user);
                    }
                    case ConsoleKey.Enter:
                    {
                        Console.CursorTop += orders.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        Console.Write($"| 确定要退 {orders[selected].RoomId} 号房 (y): ");
                        if (Console.ReadLine() != "y") break;

                        application.OrderService.CheckOutRoom(orders[selected].RoomId);

                        Console.WriteLine("| [ INFO] 退房成功");
                        return (UserLoggedMenu.Singleton, user);
                    }
                }
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }
}
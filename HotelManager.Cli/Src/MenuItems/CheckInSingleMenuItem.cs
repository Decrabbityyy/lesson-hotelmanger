using System.Text;
using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class CheckInSingleMenuItem : IMenuItem<User>
{
    public static CheckInSingleMenuItem Singleton { get; } = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "入住单人间";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        if (user == null) throw new Exception("user is null");

        Console.CursorVisible = false;
        try
        {
            Room[] rooms = application.RoomService.GetNotCheckInRooms(0, RoomType.Single, 5);
            if (rooms.Length == 0)
            {
                Console.WriteLine("| [ INFO] 没有空房了");
                return (CheckInMenu.Singleton, user);
            }

            var selected = 0;

            while (true)
            {
                Console.WriteLine("+ ------------------");
                for (var i = 0; i < rooms.Length; i++)
                    Console.WriteLine(new StringBuilder()
                        .Append("| ")
                        .Append(i == selected ? '>' : ' ')
                        .Append(' ')
                        .Append(rooms[i].Id)
                        .Append(" | ")
                        .Append(rooms[i].Type)
                        .Append(" | ")
                        .Append(rooms[i].Price));
                Console.WriteLine("+ ------------------");
                Console.Write("| ↑↓ 移动光标 ←→ 翻页 Enter 确认 q 退出");

                Console.CursorTop -= rooms.Length + 2;
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
                        selected = Math.Min(selected + 1, rooms.Length - 1);
                        break;
                    }
                    case ConsoleKey.LeftArrow:
                    {
                        Room[] prev = application.RoomService.GetNotCheckInRooms(rooms[0].Id, RoomType.Single, -5);
                        if (prev.Length == 0) break;

                        Console.CursorTop += rooms.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        rooms = prev;
                        selected = Math.Max(0, Math.Min(selected, rooms.Length));
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        Room[] next = application.RoomService.GetNotCheckInRooms(rooms[^1].Id, RoomType.Single, 5);
                        if (next.Length == 0) break;

                        Console.CursorTop += rooms.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        rooms = next;
                        selected = Math.Max(0, Math.Min(selected, rooms.Length));
                        break;
                    }
                    case ConsoleKey.Q:
                    {
                        Console.CursorTop += rooms.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        return (CheckInMenu.Singleton, user);
                    }
                    case ConsoleKey.Enter:
                    {
                        Console.CursorTop += rooms.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        Console.Write($"| 确定要入住 {rooms[selected].Id} (y): ");
                        if (Console.ReadLine() != "y") break;

                        try
                        {
                            application.OrderService.CheckInRoom(user.Name, rooms[selected].Id);
                        }
                        catch (InsufficientBalanceException)
                        {
                            Console.WriteLine("| [ERROR] 余额不足");
                            break;
                        }

                        Console.WriteLine("| [ INFO] 入住成功");
                        return (CheckInMenu.Singleton, user);
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
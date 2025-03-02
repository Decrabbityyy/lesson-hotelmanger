using System.Text;
using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;
using Microsoft.Extensions.Primitives;

namespace HotelManager.Cli.MenuItems;

internal class OverviewRoomMenuItem : IMenuItem<User>
{
    public static readonly OverviewRoomMenuItem Singleton = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return user == null ? throw new Exception("user is null") : "房间总览";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? user)
    {
        Console.CursorVisible = false;
        try
        {
            Room[] rooms = application.RoomService.GetRoomsWithOrders(0, 5);
            if (rooms.Length == 0)
            {
                Console.WriteLine("| [ INFO] 没有房间");
                return (RoomManagerMenu.Singleton, user);
            }

            var selected = 0;

            while (true)
            {
                Console.WriteLine("+ ------------------");
                for (var i = 0; i < rooms.Length; i++)
                {
                    Console.WriteLine(new StringBuilder()
                        .Append("| ")
                        .Append(i == selected ? '>' : ' ')
                        .Append(' ')
                        .Append(rooms[i].Id)
                        .Append(" | ")
                        .Append(rooms[i].Type)
                        .Append(" | ")
                        .Append(rooms[i].Price)
                        .Append(" | ")
                        .Append(rooms[i].Orders.FirstOrDefault(order => order.Status == OrderStatus.CheckedIn)?.UserId));
                }
                Console.WriteLine("+ ------------------");
                Console.Write("| ↑↓ 移动光标 ←→ 翻页 q 退出");

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
                        Room[] prev = application.RoomService.GetRooms(rooms[0].Id, -5);
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
                        Room[] next = application.RoomService.GetRooms(rooms[^1].Id, 5);
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

                        return (RoomManagerMenu.Singleton, user);
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
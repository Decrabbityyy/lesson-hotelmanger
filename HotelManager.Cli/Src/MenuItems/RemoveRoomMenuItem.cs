using System.Text;
using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class RemoveRoomMenuItem : IMenuItem<User>
{
    public static readonly RemoveRoomMenuItem Singleton = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return "删除房间";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? context)
    {
        Console.CursorVisible = false;
        try
        {
            Room[] rooms = application.RoomService.GetRooms(0, 5);
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

                        return (RoomManagerMenu.Singleton, context);
                    }
                    case ConsoleKey.Enter:
                    {
                        Console.CursorTop += rooms.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        Console.Write($"| 确定要删除 {rooms[selected].Id} (y): ");
                        if (Console.ReadLine() != "y") break;

                        application.RoomService.RemoveRoom(rooms[selected].Id);
                        return (RoomManagerMenu.Singleton, context);
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
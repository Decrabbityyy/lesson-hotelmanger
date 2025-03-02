using System.Text;
using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Models;

namespace HotelManager.Cli.MenuItems;

internal class UpgradeUserMenuItem : IMenuItem<User>
{
    public static readonly UpgradeUserMenuItem Singleton  = new();

    public string GetTitle(ApplicationContext application, User? user)
    {
        return "升级用户";
    }

    public (IMenu?, object?) Run(ApplicationContext application, User? context)
    {
        Console.CursorVisible = false;
        try
        {
            var page = 0;
            User[] users = application.UserService.GetUsers(page, 5);
            var selected = 0;

            while (true)
            {
                Console.WriteLine("+ ------------------");
                for (var i = 0; i < users.Length; i++)
                    Console.WriteLine(new StringBuilder()
                        .Append("| ")
                        .Append(i == selected ? '>' : ' ')
                        .Append(' ')
                        .Append(users[i].Name)
                        .Append(" | ")
                        .Append(users[i].Role)
                        .Append(" | ")
                        .Append(users[i].Money));
                Console.WriteLine("+ ------------------");
                Console.Write("| ↑↓ 移动光标 ←→ 翻页 Enter 确认 q 退出");

                Console.CursorTop -= users.Length + 2;
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
                        selected = Math.Min(selected + 1, users.Length - 1);
                        break;
                    }
                    case ConsoleKey.LeftArrow:
                    {
                        if (page == 0) break;

                        Console.CursorTop += users.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        page = Math.Max(page - 1, 0);
                        users = application.UserService.GetUsers(page, 5);
                        selected = Math.Max(0, Math.Min(selected, users.Length));
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        var nextPage = page + 1;

                        User[] next = application.UserService.GetUsers(page + 1, 5);
                        if (next.Length == 0) break;

                        Console.CursorTop += users.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        page = nextPage;
                        users = next;
                        selected = Math.Max(0, Math.Min(selected, users.Length));
                        break;
                    }
                    case ConsoleKey.Q:
                    {
                        Console.CursorTop += users.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        return (UserManagerMenu.Singleton, context);
                    }
                    case ConsoleKey.Enter:
                    {
                        Console.CursorTop += users.Length + 2;
                        Console.CursorLeft = 0;
                        Console.WriteLine();

                        Console.Write($"| 确定要升级 {users[selected].Name} (y): ");
                        if (Console.ReadLine() != "y") break;

                        application.UserService.UpgradeUser(users[selected].Name);
                        return (UserManagerMenu.Singleton, context);
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
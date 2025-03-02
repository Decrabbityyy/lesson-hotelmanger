using HotelManager.Cli.Menus;
using HotelManager.Cli.Utilities;
using HotelManager.Cli.Utilities.Menus;
using HotelManager.Core.Daos;
using HotelManager.Core.Models;
using HotelManager.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Cli;

internal static class Program
{
    private static void Main()
    {
        // 数据层
        DatabaseContextFactory dbFactory = new(new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite("Data Source=./db.sqlite")
            .Options);
        using (var db = dbFactory.CreateDbContext())
        {
            db.Database.EnsureCreated();
            // 检测管理员账号确定是否首次
            if (!db.Users.Any(user => user.Role == UserRole.Admin))
            {
                // 设定管理员账号
                Console.WriteLine("首次启动, 创建管理员账号");
                // 获取用户名
                string? userId;
                do
                {
                    Console.Write("请输入用户名: ");
                } while ((userId = Console.ReadLine()) == null);

                // 获取密码
                string? password;
                do
                {
                    Console.Write("请输入密码: ");
                } while ((password = Console.ReadLine()) == null);

                db.Users.Add(new User
                {
                    Name = userId,
                    Password = password,
                    Money = 0,
                    Role = UserRole.Admin
                });
                db.SaveChanges();
            }
        }

        // 业务层
        UserService userService = new(dbFactory);
        RoomService roomService = new(dbFactory);
        OrderService orderService = new(dbFactory);

        ApplicationContext application = new()
        {
            UserService = userService,
            RoomService = roomService,
            OrderService = orderService
        };

        var (menu, context) = ((IMenu)new MainMenu()).Show(application, null);
        while (menu != null)
        {
            var (nextMenu, nextContext) = menu.Show(application, context);
            if (nextMenu == null) return;
            (menu, context) = (nextMenu, nextContext);
        }
    }
}
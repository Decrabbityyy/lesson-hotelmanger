using HotelManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Core.Daos;

/// <summary>
///     数据库定义
/// </summary>
/// <param name="options">数据库选项</param>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    // 模型
    public DbSet<User> Users { get; protected set; }
    public DbSet<Room> Rooms { get; protected set; }
    public DbSet<Order> Orders { get; protected set; }
}
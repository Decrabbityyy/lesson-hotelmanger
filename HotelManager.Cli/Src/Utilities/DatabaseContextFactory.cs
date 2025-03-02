using HotelManager.Core.Daos;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Cli.Utilities;

/// <summary>
///     EF Core 自带的工厂是用于 Microsoft.Extensions.Host 的<br />
///     在这里模拟一下
/// </summary>
/// <param name="options">用于创建数据库上下文的参数</param>
public class DatabaseContextFactory(DbContextOptions<DatabaseContext> options) : IDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext()
    {
        return new DatabaseContext(options);
    }
}
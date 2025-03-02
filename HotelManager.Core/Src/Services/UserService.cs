using HotelManager.Core.Daos;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Core.Services;

/// <summary>
///     用户相关业务
/// </summary>
public class UserService(IDbContextFactory<DatabaseContext> dbFactory)
{
    private readonly IDbContextFactory<DatabaseContext> _dbFactory = dbFactory;

    /// <summary>
    ///     创建一个用户
    /// </summary>
    /// <param name="id">用户 Id</param>
    /// <param name="password">用户密码</param>
    /// <param name="role">用户等级</param>
    /// <returns>用户</returns>
    /// <exception cref="UserExistsException">此用户名已被使用</exception>
    public User CreateUser(string id, string password, UserRole role)
    {
        using var db = _dbFactory.CreateDbContext();
        if (db.Users.Any(user => user.Name == id)) throw new UserExistsException(id);

        User user = new()
        {
            Name = id,
            Password = password,
            Money = 0,
            Role = role
        };
        db.Users.Add(user);
        db.SaveChanges();

        return user;
    }

    /// <summary>
    ///     移除用户
    /// </summary>
    /// <param name="id">用户名</param>
    /// <exception cref="UserNotFoundException">没有找到此用户名对应的用户</exception>
    public void RemoveUser(string id)
    {
        using var db = _dbFactory.CreateDbContext();
        var user = db.Users.FirstOrDefault(user => user.Name == id) ?? throw new UserNotFoundException(id);

        db.Users.Remove(user);
        db.SaveChanges();
    }

    /// <summary>
    ///     升级用户
    /// </summary>
    /// <param name="id">用户名</param>
    /// <exception cref="UserNotFoundException">没有找到此用户名对应的用户</exception>
    public void UpgradeUser(string id)
    {
        using var db = _dbFactory.CreateDbContext();
        var user = db.Users.FirstOrDefault(user => user.Name == id && user.Role != UserRole.Admin)
                   ?? throw new UserNotFoundException(id);

        user.Role = (UserRole)Math.Min((int)user.Role + 1, 5);
        db.SaveChanges();
    }

    /// <summary>
    ///     降级用户
    /// </summary>
    /// <param name="id">用户名</param>
    /// <exception cref="UserNotFoundException">没有找到此用户名对应的用户</exception>
    public void DowngradeUser(string id)
    {
        using var db = _dbFactory.CreateDbContext();
        var user = db.Users.FirstOrDefault(user => user.Name == id && user.Role != UserRole.Admin)
                   ?? throw new UserNotFoundException(id);

        user.Role = (UserRole)Math.Max((int)user.Role - 1, 0);
        db.SaveChanges();
    }

    /// <summary>
    ///     充值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="money"></param>
    /// <exception cref="UserNotFoundException">没有找到此用户名对应的用户</exception>
    public void RechargeUser(string id, long money)
    {
        using var db = _dbFactory.CreateDbContext();
        var user = db.Users.FirstOrDefault(user => user.Name == id && user.Role != UserRole.Admin)
                   ?? throw new UserNotFoundException(id);

        user.Money += money;
        db.SaveChanges();
    }

    /// <summary>
    ///     检查用户是否存在
    /// </summary>
    /// <param name="id">用户名</param>
    /// <returns></returns>
    public bool UserExists(string id)
    {
        using var db =_dbFactory.CreateDbContext();
        return db.Users.Any(user => user.Name == id);
    }

    /// <summary>
    ///     获取一个用户
    /// </summary>
    /// <param name="name">用户名</param>
    public User? GetUser(string name)
    {
        using var db = _dbFactory.CreateDbContext();

        return db.Users.FirstOrDefault(user => user.Name == name);
    }

    /// <summary>
    ///     获取分页用户
    /// </summary>
    /// <param name="page">当前页</param>
    /// <param name="count">数量</param>
    public User[] GetUsers(int page, int count)
    {
        using var db = _dbFactory.CreateDbContext();

        return [.. db.Users.Skip(page * count).Take(count)];
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="name">用户名</param>
    /// <param name="newPassword">新密码</param>
    /// <exception cref="UserNotFoundException">没有找到此用户名对应的用户</exception>
    public void ChangePassword(string name, string newPassword)
    {
        using var db = _dbFactory.CreateDbContext();
        var user = db.Users.FirstOrDefault(user => user.Name == name) ?? throw new UserNotFoundException(name);
        user.Password = newPassword;
        db.SaveChanges();
    }
}
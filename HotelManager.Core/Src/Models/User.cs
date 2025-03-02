using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Core.Models;

/// <summary>
///     用户表
/// </summary>
[Table("user")] // 表名称
[Index(nameof(Role))] // 索引
public class User
{
    /// <summary>
    ///     用户名
    /// </summary>
    [Column("id")] // 列名
    [Key] // 主键
    // https://learn.microsoft.com/zh-cn/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
    public required string Name { get; init; }

    // 一般用户密码都是储存加盐后的 hash
    // 防止明文密码泄露
    // 此处先使用明文

    /// <summary>
    ///     用户密码
    /// </summary>
    [Column("password")]
    public required string Password { get; set; }

    /// <summary>
    ///     用户角色
    /// </summary>
    [Column("role")]
    public required UserRole Role { get; set; }

    // 通常钱使用整数储存精度为 2 的小数
    // 即单位角
    // 此处就没做此操作

    /// <summary>
    ///     余额
    ///     <br/>
    ///         单位元
    /// </summary>
    [Column("money")]
    public required long Money { get; set; }
}

/// <summary>
///     用户角色
/// </summary>
public enum UserRole
{
    Admin = -1,
    Normal,
    VIP1,
    VIP2,
    VIP3,
    VIP4,
    VIP5
}
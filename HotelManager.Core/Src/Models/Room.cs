using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManager.Core.Models;

/// <summary>
///     房间表
/// </summary>
[Table("room")] // 表名
public class Room
{
    /// <summary>
    ///     房间号
    /// </summary>
    [Column("id")] // 列名
    // [Key] // 主键, EF Core 约定使用 Id 或 <type name>Id 的属性为主键
    // https://learn.microsoft.com/zh-cn/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
    public required long Id { get; init; }

    /// <summary>
    ///     房间类型
    /// </summary>
    [Column("type")]
    public required RoomType Type { get; init; }

    // 通常钱使用整数储存精度为 2 的小数
    // 即单位角
    // 此处就没做此操作

    /// <summary>
    ///     价格
    /// <br />
    ///     单位元
    /// </summary>
    [Column("price")]
    public required long Price { get; set; }

    /// <summary>
    ///     EF Core 的联表查询
    /// </summary>
    public List<Order> Orders { get; init; } = [];

}

public enum RoomType
{
    Single,
    Double,
    Suite
}
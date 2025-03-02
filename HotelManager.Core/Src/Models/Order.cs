using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManager.Core.Models;

/// <summary>
///     订单表
/// </summary>
[Table("order")]
public class Order
{
    /// <summary>
    ///     订单号
    /// </summary>
    [Column("id")] // 列名
    // [Key] // 主键, EF Core 约定使用 Id 或 <type name>Id 的属性为主键
    // https://learn.microsoft.com/zh-cn/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 自动递增主键
    public long Id { get; init; }

    /// <summary>
    ///     用户名
    /// </summary>
    [Column("user_id")]
    [ForeignKey(nameof(User))] // 外键
    public required string UserId { get; init; }

    /// <summary>
    ///     房间号
    /// </summary>
    [Column("room_id")]
    [ForeignKey(nameof(Room))]
    public required long RoomId { get; init; }

    /// <summary>
    ///     EF CORE 的联表查询
    /// </summary>
    public Room Room { get; init; } = null!;

    /// <summary>
    ///     订单状态
    /// </summary>
    public required OrderStatus Status { get; set; }
}

/// <summary>
///     订单状态
/// </summary>
public enum OrderStatus
{
    CheckedIn,
    CheckedOut,

}
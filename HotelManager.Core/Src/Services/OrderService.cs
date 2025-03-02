using HotelManager.Core.Daos;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Core.Services;

public class OrderService(IDbContextFactory<DatabaseContext> dbFactory)
{
    private readonly IDbContextFactory<DatabaseContext> _dbFactory = dbFactory;

    // 单线程就不加锁检查了

    /// <summary>
    ///     入住房间
    /// </summary>
    /// <param name="userId">用户名</param>
    /// <param name="roomId">房间名</param>
    /// <returns></returns>
    /// <exception cref="UserNotFoundException">用户未找到</exception>
    /// <exception cref="RoomNotFoundException">房间未找到</exception>
    public Order CheckInRoom(string userId, long roomId)
    {
        using var db = _dbFactory.CreateDbContext();

        var user = db.Users.FirstOrDefault(user => user.Name == userId) ?? throw new UserNotFoundException(userId);
        var room = db.Rooms.FirstOrDefault(room => room.Id == roomId) ?? throw new RoomNotFoundException(roomId);

        if (db.Orders.Any(order => order.RoomId == roomId && order.Status == OrderStatus.CheckedIn)) throw new RoomCheckedInException(roomId);

        if (user.Money < room.Price) throw new InsufficientBalanceException(userId);

        Order order = new()
        {
            UserId = userId,
            RoomId = roomId,
            Status = OrderStatus.CheckedIn
        };
        db.Orders.Add(order);
        user.Money -= room.Price;
        db.SaveChanges();

        return order;
    }

    /// <summary>
    ///     退房
    /// </summary>
    /// <param name="roomId">房间名</param>
    /// <returns></returns>
    /// <exception cref="RoomNotFoundException">房间未找到</exception>
    public Order CheckOutRoom(long roomId)
    {
        using var db = _dbFactory.CreateDbContext();

        var room = db.Rooms.FirstOrDefault(room => room.Id == roomId) ?? throw new RoomNotFoundException(roomId);

        var order = db.Orders.FirstOrDefault(order => order.RoomId == roomId && order.Status == OrderStatus.CheckedIn)
                    ?? throw new RoomCheckedOutException(roomId);

        order.Status = OrderStatus.CheckedOut;
        db.SaveChanges();

        return order;
    }

    public Order[] GetCheckInOrders(long roomId, int count)
    {
        using var db = _dbFactory.CreateDbContext();

        IQueryable<Order> orders = db.Orders
            .Where(order => order.Status == OrderStatus.CheckedIn)
            .Include(order => order.Room);

        return count switch
        {
            < 0 => [.. orders.Where(room => room.Id < roomId).OrderByDescending(room => room.Id).Take(-count)],
            > 0 => [.. orders.Where(room => room.Id > roomId).OrderBy(room => room.Id).Take(-count)],
            _ => throw new NotImplementedException()
        };
    }
}
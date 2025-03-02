using HotelManager.Core.Daos;
using HotelManager.Core.Exceptions;
using HotelManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Core.Services;

/// <summary>
///     房间相关业务
/// </summary>
/// <param name="dbFactory"></param>
public class RoomService(IDbContextFactory<DatabaseContext> dbFactory)
{
    private readonly IDbContextFactory<DatabaseContext> _dbFactory = dbFactory;

    /// <summary>
    ///     新增房间
    /// </summary>
    /// <param name="id">房间号</param>
    /// <param name="type">房间类型</param>
    /// <param name="price">房间价格</param>
    /// <exception cref="RoomExistsException">此房间号已被使用</exception>
    public Room CreateRoom(long id, RoomType type, long price)
    {
        using var db = _dbFactory.CreateDbContext();
        if (db.Rooms.Any(room => room.Id == id)) throw new RoomExistsException(id);

        Room room = new()
        {
            Id = id,
            Type = type,
            Price = price
        };
        db.Rooms.Add(room);
        db.SaveChanges();

        return room;
    }

    /// <summary>
    ///     删除房间
    /// </summary>
    /// <param name="id">房间号</param>
    /// <exception cref="RoomNotFoundException">没有找到此房间号对应的房间</exception>
    public void RemoveRoom(long id)
    {
        using var db = _dbFactory.CreateDbContext();

        var room = db.Rooms.FirstOrDefault(room => room.Id == id) ?? throw new RoomNotFoundException(id);

        db.Rooms.Remove(room);
        db.SaveChanges();
    }

    // /// <summary>
    // /// 获取指定数量房间
    // /// </summary>
    // /// <returns>指定数量的房间信息</returns>
    // public async Task<Room[]> GetAllRooms(int count) {
    //     using var db = await _dbFactory.CreateDbContextAsync();
    //     return await db.Rooms.Take(count).ToArrayAsync();
    // }

    // /// <summary>
    // /// 从指定序号开始获取指定数量房间
    // /// </summary>
    // /// <returns>从指定序号开始的指定数量的房间信息</returns>
    // public async Task<Room[]> GetAllRooms(ulong id, int count) {
    //     using var db = await _dbFactory.CreateDbContextAsync();
    //     return await db.Rooms.OrderBy(room => room.Id).SkipWhile(room => room.Id < id).Take(count).ToArrayAsync();
    // }

    /// <summary>
    ///     根据房间号查询房间
    /// </summary>
    /// <param name="id">房间号</param>
    public Room? GetRoom(long id)
    {
        using var db = _dbFactory.CreateDbContext();
        return db.Rooms.FirstOrDefault(room => room.Id == id);
    }

    /// <summary>
    /// 获取房间
    /// </summary>
    /// <param name="index">房间游标</param>
    /// <param name="count">数量</param>
    /// <returns>房间</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Room[] GetRooms(long index, int count)
    {
        using var db = _dbFactory.CreateDbContext();

        return count switch
        {
            < 0 => [.. db.Rooms.Where(room => room.Id < index).OrderByDescending(room => room.Id).Take(-count)],
            > 0 => [.. db.Rooms.Where(room => room.Id > index).OrderBy(room => room.Id).Take(count)],
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// 获取未入住房间
    /// </summary>
    /// <param name="index">房间游标</param>
    /// <param name="type">房间类型</param>
    /// <param name="count">数量</param>
    /// <returns>房间</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Room[] GetNotCheckInRooms(long index, RoomType type, int count)
    {
        using var db = _dbFactory.CreateDbContext();

        IQueryable<Room> rooms = db.Rooms
            .Where(room => room.Type == type)
            .Include(room => room.Orders)
            .Where(room => room.Orders.All(order => order.Status != OrderStatus.CheckedIn));

        return count switch
        {
            < 0 => [.. rooms.Where(room => room.Id < index).OrderByDescending(room => room.Id).Take(-count)],
            > 0 => [.. rooms.Where(room => room.Id > index).OrderBy(room => room.Id).Take(-count)],
            _ => throw new NotImplementedException()
        };
    }

    public Room[] GetRoomsWithOrders(long index, int count)
    {
        using var db = _dbFactory.CreateDbContext();

        IQueryable<Room> rooms = db.Rooms.Include(room => room.Orders);

        return count switch
        {
            < 0 => [.. rooms.Where(room => room.Id < index).OrderByDescending(room => room.Id).Take(-count)],
            > 0 => [.. rooms.Where(room => room.Id > index).OrderBy(room => room.Id).Take(count)],
            _ => throw new NotImplementedException()
        };

    }
    
}
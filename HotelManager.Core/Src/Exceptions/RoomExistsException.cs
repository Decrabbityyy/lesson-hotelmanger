namespace HotelManager.Core.Exceptions;

public class RoomExistsException(long id) : Exception($"房间 ({id}) 已存在")
{
    private long Id { get; } = id;
}
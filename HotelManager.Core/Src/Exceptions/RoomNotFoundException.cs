namespace HotelManager.Core.Exceptions;

public class RoomNotFoundException(long id) : Exception($"房间 ({id}) 不存在")
{
    private long Id { get; } = id;
}
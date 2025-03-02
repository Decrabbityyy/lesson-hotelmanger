namespace HotelManager.Core.Exceptions;

public class RoomCheckedOutException(long id) : Exception($"房间 ({id}) 未入住")
{
    private long Id { get; } = id;
}
namespace HotelManager.Core.Exceptions;

public class RoomCheckedInException(long id) : Exception($"房间 ({id}) 已入住")
{
    private long Id { get; } = id;
}
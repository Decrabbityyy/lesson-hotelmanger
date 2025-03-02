namespace HotelManager.Core.Exceptions;

public class UserNotFoundException(string id) : Exception($"用户 ({id}) 不存在")
{
    private string Id { get; } = id;
}
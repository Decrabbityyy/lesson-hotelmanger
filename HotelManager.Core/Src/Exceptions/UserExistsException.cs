namespace HotelManager.Core.Exceptions;

public class UserExistsException(string id) : Exception($"用户 ({id}) 已存在")
{
    private string Id { get; } = id;
}
namespace HotelManager.Core.Exceptions;

public class InsufficientBalanceException(string id) : Exception($"用户 ({id}) 余额不足")
{
    private string Id { get; } = id;
}
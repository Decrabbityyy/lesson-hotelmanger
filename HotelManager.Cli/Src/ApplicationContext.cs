using HotelManager.Core.Services;

namespace HotelManager.Cli;

internal class ApplicationContext
{
    public required UserService UserService { get; init; }

    public required RoomService RoomService { get; init; }

    public required OrderService OrderService { get; init; }
}
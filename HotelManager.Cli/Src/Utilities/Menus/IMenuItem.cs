namespace HotelManager.Cli.Utilities.Menus;

internal interface IMenuItem
{
    string GetTitle(ApplicationContext application, object? context);

    (IMenu?, object?) Run(ApplicationContext application, object? context);
}